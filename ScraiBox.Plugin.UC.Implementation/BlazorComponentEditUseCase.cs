using Microsoft.VisualBasic.FileIO;
using ScraiBox.Core;
using ScraiBox.Core.Interfaces;
using ScraiBox.Core.Interfaces.DTO;
using System.Text;

namespace ScraiBox.Plugin.UC.Implementation
{
    public class BlazorComponentEditUseCase : IUseCasePlugin
    {
        private readonly RoslynService _roslynService;
        private readonly InventoryService _inventoryService;

        public BlazorComponentEditUseCase(InventoryService inventory, RoslynService roslyn)
        {
            _roslynService = roslyn;
            _inventoryService = inventory;
        }

        public string Name => "BlazorEdit";
        public string Description => @"Automatically gathers the .razor file, its code-behind (.razor.cs), and all referenced DTOs or Models using Roslyn analysis. Ideal for providing full context when modifying UI components.";

        public async Task<ScraiBoxResult> ExecuteAsync(ScraiBoxContext context)
        {
            // 1. Get the target component name from context (e.g., "Home")
            var componentName = context.RawInput;
            var inventory = context.Inventory;

            // 2. Find the primary files (.razor and .razor.cs)
            var primaryFiles = _inventoryService.Search(inventory, componentName)
                .Where(f => f.Name.EndsWith(".razor") || f.Name.EndsWith(".razor.cs"))
                .ToList();

            var resultContext = new StringBuilder();
            var allFoundFiles = new List<string>();

            foreach (var file in primaryFiles)
            {
                var fullPath = Path.Combine(context.ProjectRootPath, file.RelativePath);
                var content = await File.ReadAllTextAsync(fullPath);

                resultContext.AppendLine($"--- FILE: {file.RelativePath} ---");
                resultContext.AppendLine(content);
                allFoundFiles.Add(file.RelativePath);

                // 3. If it's a C# file, analyze it for dependencies
                if (file.Type == FileType.CSharp)
                {
                    var dependencies = await _roslynService.GetUsedTypeNamesAsync(content);
                    var depFiles = _inventoryService.FindFilesByClassNames(inventory, dependencies);

                    foreach (var dep in depFiles)
                    {
                        if (allFoundFiles.Contains(dep.RelativePath)) continue;

                        var depPath = Path.Combine(context.ProjectRootPath, dep.RelativePath);
                        var depContent = await File.ReadAllTextAsync(depPath);

                        resultContext.AppendLine($"--- DEPENDENCY: {dep.RelativePath} ---");
                        resultContext.AppendLine(depContent);
                        allFoundFiles.Add(dep.RelativePath);
                    }
                }
            }

            return new ScraiBoxResult
            {
                IsSuccess = true,
                OutputData = resultContext.ToString(),
                Message = $"Aggregated {allFoundFiles.Count} files for component {componentName}"
            };
        }
    }
}
