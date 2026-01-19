using ScraiBox.Core;
using ScraiBox.Core.Interfaces;
using ScraiBox.Core.Interfaces.DTO;
using System.Text;

namespace ScraiBox.Plugin.UC.Analysis
{

    public class MethodCallTreeUseCase : IUseCasePlugin
    {
        protected readonly RoslynService _roslynService;
        protected readonly InventoryService _inventoryService;

        public MethodCallTreeUseCase(InventoryService inventory, RoslynService roslyn)
        {
            _roslynService = roslyn;
            _inventoryService = inventory;
        }

        public virtual string Name => "MethodCallTree";

        public virtual string Description => "Recursively traces method calls starting from a specific class method. It builds a hierarchical tree of invocations to visualize execution flow across different services and layers, helping AI understand complex business logic dependencies.";

        public async Task<ScraiBoxResult> ExecuteAsync(ScraiBoxContext context)
        {
            // Expected format: Namespace.SubNamespace.ClassName.MethodName
            var target = context.RawInput;
            var parts = target.Split('.');

            if (parts.Length < 2)
            {
                return new ScraiBoxResult { IsSuccess = false, Message = "Target must be in 'ClassName.MethodName' format." };
            }

            string methodName = parts[^1];
            string className = parts[^2];

            var inventory = context.Inventory;
            var resultBuilder = new StringBuilder();
            var visitedMethods = new HashSet<string>();

            resultBuilder.AppendLine($"# Method Call Tree for: {target}");

            await TraceCallsRecursive(className, methodName, inventory, context.ProjectRootPath, resultBuilder, visitedMethods, 0);

            return new ScraiBoxResult
            {
                IsSuccess = true,
                OutputData = resultBuilder.ToString(),
                Message = $"Call tree generated for {methodName}."
            };
        }

        protected virtual async Task TraceCallsRecursive(string className, string methodName, ProjectInventory inventory, string rootPath, StringBuilder sb, HashSet<string> visited, int depth)
        {
            string key = $"{className}.{methodName}";
            if (visited.Contains(key) || depth > 5) return; // Increased depth for enterprise code 
            visited.Add(key);

            var indentation = new string(' ', depth * 4);
            sb.AppendLine($"{indentation}* {className}.{methodName}");

            var files = _inventoryService.FindFilesByClassNames(inventory, new[] { className });
            var file = files.FirstOrDefault();
            if (file == null) return;

            var fullPath = Path.Combine(rootPath, file.RelativePath);
            if (!File.Exists(fullPath)) return;

            var code = await File.ReadAllTextAsync(fullPath);
            var calls = await _roslynService.GetMethodCallsAsync(code, className, methodName);

            foreach (var call in calls)
            {
                string nextClass = className; // Default to current class for internal calls
                string nextMethod = "";

                // Remove "await " if present
                var cleanCall = call.Replace("await ", "").Trim();
                var callParts = cleanCall.Split('.');

                if (callParts.Length >= 2)
                {
                    // Case: instance.Method() or this.Method()
                    nextClass = callParts[0]
                        .Replace("this", className)
                        .Replace("_", ""); // Heuristic for private fields like _walletService -> WalletService

                    // Basic cleanup for method name (remove arguments)
                    nextMethod = callParts[1].Split('(')[0];
                }
                else
                {
                    // Case: Method() -> internal call within the same class
                    nextMethod = callParts[0].Split('(')[0];
                }

                if (!string.IsNullOrEmpty(nextMethod))
                {
                    await TraceCallsRecursive(nextClass, nextMethod, inventory, rootPath, sb, visited, depth + 1);
                }
            }
        }
    }
}
