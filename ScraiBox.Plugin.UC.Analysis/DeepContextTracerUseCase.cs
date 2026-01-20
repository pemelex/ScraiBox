using ScraiBox.Core;
using ScraiBox.Core.Interfaces;
using ScraiBox.Core.Interfaces.DTO;
using System.Reflection;
using System.Text;

namespace ScraiBox.Plugin.UC.Implementation
{
    public class DeepContextTracerUseCase : IUseCasePlugin
    {
        private readonly AdvancedRoslynService _roslynService;
        private readonly int _maxDepth = 4; // Bezpečnostní pojistka proti nekonečné rekurzi

        public string Name => "DeepContextTracer";

        public string Description => "Recursively traces method calls starting from a specific class method including method bodies.";


        public DeepContextTracerUseCase(AdvancedRoslynService roslynService)
        {
            _roslynService = roslynService;
        }

        public async Task<ScraiBoxResult> ExecuteAsync(ScraiBoxContext context)
        {
            // Expected format: Namespace.SubNamespace.ClassName.MethodName
            var target = context.RawInput;
            var parts = target.Split('.');

            if (parts.Length < 2)
            {
                return new ScraiBoxResult { IsSuccess = false, Message = "Target must be in 'ClassName.MethodName' format." };
            }

            string startMethod = parts[^1];
            string startClass = parts[^2];

            _roslynService.InitializeFromInventory(context.Inventory!, context.ProjectRootPath);

            var visitedMethods = new HashSet<string>();

            string projectName = new DirectoryInfo(context.ProjectRootPath).Name;
            using var traceOutput = new TraceWriter(projectName);

            traceOutput.AppendLine($"# DEEP CONTEXT TRACE: {startClass}.{startMethod}");
            traceOutput.AppendLine($"Generated: {DateTime.Now}");
            traceOutput.AppendLine("---");

            await TraceRecursive(startClass, startMethod, 0, visitedMethods, traceOutput);

            var outputData = traceOutput.ToString();
            // Implementace uložení do souboru podle backlogu
            File.WriteAllText(Path.Combine(context.ProjectRootPath, "DeepContextTrace.txt"), outputData);

            return new ScraiBoxResult
            {
                IsSuccess = true,
                OutputData = outputData,
                Message = $"Call tree generated for {startClass}{startMethod}."
            };
        }

        private async Task TraceRecursive(string className, string methodName, int depth, HashSet<string> visited, TraceWriter output)
        {
            string fullId = $"{className}.{methodName}";

            // Ochrana proti nekonečné rekurzi a limit hloubky
            if (depth > _maxDepth || visited.Contains(fullId)) return;
            visited.Add(fullId);

            var indent = new string(' ', depth * 2);

            // 1. Získáme detailní informace o metodě skrze sémantický model
            // Předpokládáme, že jsme do AdvancedRoslynService přidali metodu GetMethodDetailsByNameAsync
            var methodDetails = await _roslynService.GetMethodDetailsByNameAsync(className, methodName);

            if (methodDetails == null)
            {
                // Metodu se nepodařilo v solution najít (může jít o dynamické volání nebo chybu v indexu)
                output.AppendLine($"{indent}## METHOD: {fullId} (NOT FOUND)");
                return;
            }

            // 2. Vypíšeme hlavičku a kód, pokud je lokální
            if (methodDetails.Value.IsLocal)
            {
                output.AppendLine($"{indent}## METHOD: {fullId} (LOCAL)");
                if (!string.IsNullOrEmpty(methodDetails.Value.SourceCode))
                {
                    output.AppendLine($"{indent}```csharp");
                    // Přidáme odsazení i pro samotný kód, aby byl Markdown přehledný
                    var indentedSource = string.Join("\n", methodDetails.Value.SourceCode.Split('\n').Select(line => indent + line));
                    output.AppendLine(indentedSource);
                    output.AppendLine($"{indent}```");
                }

                // 3. Pokračujeme v rekurzi POUZE u lokálního kódu
                var calls = await _roslynService.GetDeepMethodCallsAsync(className, methodName);
                foreach (var call in calls)
                {
                    // Ignorujeme volání, která už známe jako systémová (např. String.IsNullOrEmpty)
                    // nebo ta, která neobsahují tečku (nevalidní formát)
                    if (!call.Contains('.') || IsIgnoredSystemNamespace(call))
                    {
                        if (!IsIgnoredSystemNamespace(call))
                            output.AppendLine($"{indent}  -> CALL: {call} (EXTERNAL)");
                        continue;
                    }

                    var parts = call.Split('.');
                    string nextClass = parts[0];
                    string nextMethod = parts[1];

                    await TraceRecursive(nextClass, nextMethod, depth + 1, visited, output);
                }
            }
            else
            {
                // Externí volání (Framework, NuGet) – jen zapíšeme, že proběhlo
                output.AppendLine($"{indent}## METHOD: {fullId} (EXTERNAL/SYSTEM)");
            }
        }

        // Pomocná metoda pro odfiltrování nejčastějšího balastu
        private bool IsIgnoredSystemNamespace(string call)
        {
            return call.StartsWith("System.") ||
                   call.StartsWith("Microsoft.") ||
                   call.StartsWith("String.") ||
                   call.StartsWith("List.");
        }
    }
}