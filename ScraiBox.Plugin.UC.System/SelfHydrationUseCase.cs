using ScraiBox.Core.Interfaces;
using ScraiBox.Core.Interfaces.DTO;
using System.Text;

namespace ScraiBox.Plugin.UC.System
{
    public class SelfHydrationUseCase : IUseCasePlugin
    {
        public string Name => "SelfHydration";
        public string Description => "Generates a state summary to 're-hydrate' the AI's context in a new thread.";

        public async Task<ScraiBoxResult> ExecuteAsync(ScraiBoxContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# SCRAIBOX SESSION RESUMPTION ANCHOR");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            sb.AppendLine();

            sb.AppendLine("## 1. Project Context");
            sb.AppendLine($"Project: {new DirectoryInfo(context.ProjectRootPath).Name}");
            sb.AppendLine($"Time: {DateTime.Now:G}");
            sb.AppendLine($"- Root: `{context.ProjectRootPath}`");

            sb.AppendLine("\n--- INVENTORY SUMMARY ---");
            var projects = context.Inventory != null ? 
                context.Inventory.Files.Select(f => f.ProjectName).Distinct().ToList() :
                new List<string>();

            sb.AppendLine($"- Detected Projects: {string.Join(", ", projects)}");
            sb.AppendLine();

            // Cesta k souboru v rootu
            string statusFilePath = Path.Combine(context.ProjectRootPath, "Status.scbs");
            string statusContent = "No status file found. Please create Status.scbs in root.";

            if (File.Exists(statusFilePath))
            {
                statusContent = await File.ReadAllTextAsync(statusFilePath);
            }

            sb.AppendLine("\n--- PROJECT STATUS ---");
            sb.AppendLine(statusContent);

            return new ScraiBoxResult
            {
                IsSuccess = true,
                Message = "Hydration anchor generated. Copy the output to a new chat thread.",
                OutputData = sb.ToString()
            };
        }
    }
}
