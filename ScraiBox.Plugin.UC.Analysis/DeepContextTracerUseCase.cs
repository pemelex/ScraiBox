using ScraiBox.Core;
using ScraiBox.Core.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Plugin.UC.Analysis
{
    public class DeepContextTracerUseCase : MethodCallTreeUseCase
    {
        public DeepContextTracerUseCase(InventoryService inventory, RoslynService roslyn)
            : base(inventory, roslyn) { }

        public override string Name => "DeepContextTracer";

        public override string Description => "Recursively traces method calls starting from a specific class method including method bodies.";


        protected override async Task TraceCallsRecursive(string className, string methodName, ProjectInventory inventory, string rootPath, StringBuilder sb, HashSet<string> visited, int depth)
        {
            string key = $"{className}.{methodName}";
            if (visited.Contains(key) || depth > 5) return;
            visited.Add(key);

            var files = _inventoryService.FindFilesByClassNames(inventory, new[] { className });
            var file = files.FirstOrDefault();
            if (file == null) return;

            var fullPath = Path.Combine(rootPath, file.RelativePath);
            if (!File.Exists(fullPath)) return;

            var code = await File.ReadAllTextAsync(fullPath);

            // Extrakce zdrojáku aktuální metody
            var methodCode = await _roslynService.GetMethodSourceCodeAsync(code, className, methodName);
            if (!string.IsNullOrEmpty(methodCode))
            {
                sb.AppendLine($"\n--- SOURCE: {className}.{methodName} (Depth: {depth}) ---");
                sb.AppendLine(methodCode);
                sb.AppendLine("-------------------------------------------\n");
            }

            // Získání seznamu volání z této metody
            var calls = await _roslynService.GetMethodCallsAsync(code, className, methodName);

            foreach (var call in calls)
            {
                string nextClass = className; // Výchozí: volání v rámci stejné třídy
                string nextMethod = "";

                // Vylepšená část uvnitř foreach (var call in calls)
                var cleanCall = call.Replace("await ", "").Trim();
                var callParts = cleanCall.Split('.');

                if (callParts.Length >= 2)
                {
                    // 1. Zkusíme zjistit, zda je to volání na poli (např. _inventoryService.Find...)
                    string instanceName = callParts[0].TrimStart('_');

                    // 2. Agresivní pokus o nalezení třídy:
                    // Zkusíme vzít název instance a udělat z něj PascalCase (častý pattern pro DI)
                    string guessedClass = char.ToUpper(instanceName[0]) + instanceName.Substring(1);

                    // 3. Ověření v Inventory - pokud guessedClass neexistuje, 
                    // zkusíme, jestli callParts[0] není přímo název třídy (statické volání)
                    if (_inventoryService.FindFilesByClassNames(inventory, new[] { guessedClass }).Any())
                    {
                        nextClass = guessedClass;
                    }
                    else if (_inventoryService.FindFilesByClassNames(inventory, new[] { callParts[0] }).Any())
                    {
                        nextClass = callParts[0];
                    }

                    nextMethod = callParts[1].Split('(')[0];
                }
                else
                {
                    // Jednoduché volání: Metoda()
                    nextMethod = callParts[0].Split('(')[0];
                }

                if (!string.IsNullOrEmpty(nextMethod))
                {
                    // Rekurzivní skok do další metody
                    await TraceCallsRecursive(nextClass, nextMethod, inventory, rootPath, sb, visited, depth + 1);
                }
            }
        }
    }
}
