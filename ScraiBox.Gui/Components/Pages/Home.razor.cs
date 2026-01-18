using CommunityToolkit.Maui.Storage;
using ScraiBox.Core;
using ScraiBox.Core.Interfaces.DTO;
using ScraiBox.Plugin.UC.Implementation;
using ScraiBox.Plugin.UC.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Gui.Components.Pages
{
    public partial class Home
    {
        private string ProjectPath = "";
        private string AiInput = "";
        private string SelectedUseCase = "SelfHydration";
        private string UseCaseTarget = "";
        private List<string> Logs = new();

        // Manual instance since we are in a simple setup
        private CommandInterceptor Interceptor = new();

        private InventoryService InventoryService { get; init; }

        private RoslynService RoslynService { get; init; }

        public Home(InventoryService inventoryService, RoslynService roslynService)
        {
            InventoryService = inventoryService;
            RoslynService = roslynService;
        }

        private async Task PickFolder()
        {
            try
            {
                var result = await FolderPicker.Default.PickAsync(CancellationToken.None);
                if (result.IsSuccessful)
                {
                    ProjectPath = result.Folder.Path;
                    Logs.Add("Project root linked successfully.");
                    await GenerateJsonMap();
                }
            }
            catch (Exception ex)
            {
                Logs.Add($"❌ Browser Error: {ex.Message}");
            }
        }

        private async Task GenerateJsonMap()
        {
            try
            {
                var mapper = new ProjectMapper(ProjectPath);
                string jsonMap = mapper.GenerateJsonMap();

                // Adding instructions for the AI directly into the JSON or a companion file
                string fullOutput = $"# INSTRUCTIONS\n" +
                                   $"If you need full code of any file, reply with: <!cmd:scry:relative_path>\n\n" +
                                   $"# PROJECT JSON MAP\n{jsonMap}";

                string path = Path.Combine(ProjectPath, "project_map.md");
                await File.WriteAllTextAsync(path, fullOutput);

                // Also copy the map to clipboard immediately so you can start the thread
                await Clipboard.Default.SetTextAsync(fullOutput);

                Logs.Add("✅ Project Map (JSON + Instructions) generated and copied to clipboard!");
            }
            catch (Exception ex)
            {
                Logs.Add($"❌ Mapper Error: {ex.Message}");
            }
        }

        private async Task ExecuteSelectedUseCase()
        {
            if (string.IsNullOrEmpty(ProjectPath))
            {
                Logs.Add("⚠️ Select project folder first!");
                return;
            }

            try
            {
                var sbctx = new ScraiBoxContext
                {
                    ProjectRootPath = ProjectPath,
                    RawInput = UseCaseTarget,
                    Inventory = InventoryService.BuildInventory(ProjectPath)
                };

                // Tady zavoláš tu logiku, kterou jsme vymysleli
                // Pro začátek to může být přímo v code-behind, než dořešíme DI
                if (SelectedUseCase == "SelfHydration")
                {
                    var uc = new SelfHydrationUseCase();
                    var result = await uc.ExecuteAsync(sbctx);
                    await Clipboard.Default.SetTextAsync(result.OutputData);
                    Logs.Add("✅ Hydration anchor copied to clipboard!");
                }
                else if (SelectedUseCase == "BlazorEdit")
                {
                    var uc = new BlazorComponentEditUseCase(InventoryService, RoslynService);
                    var result = await uc.ExecuteAsync(sbctx);
                    await Clipboard.Default.SetTextAsync(result.OutputData);
                    Logs.Add("✅ Blazor contextual data copied to clipboard!");
                }
            }
            catch (Exception ex)
            {
                Logs.Add($"❌ UC Error: {ex.Message}");
            }
        }

        private async Task ProcessCommand()
        {
            if (string.IsNullOrWhiteSpace(ProjectPath))
            {
                Logs.Add("⚠️ Error: Select project folder first!");
                return;
            }

            if (string.IsNullOrWhiteSpace(AiInput)) return;

            // 1. Zachytíme všechny příkazy v textu
            var commands = Interceptor.InterceptAll(AiInput);

            if (commands.Count == 0)
            {
                Logs.Add("⚠️ No valid command found. Check the format: <!cmd:name:param>");
                return;
            }

            // 2. Odfiltrujeme cesty pro 'scry'
            var scryPaths = commands
                .Where(c => c.Name.Equals("scry", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Parameter)
                .ToList();

            if (scryPaths.Any())
            {
                var scryer = new ContextScryer(ProjectPath);

                // 3. Použijeme novou metodu pro hromadné čtení
                string content = scryer.ScryMultiple(scryPaths);

                // 4. Uložíme výsledek do schránky
                await Clipboard.Default.SetTextAsync(content);

                Logs.Add($"🚀 Success! Context for {scryPaths.Count} items is in your clipboard.");

                // Vyčistíme vstup, jen pokud jsme vše úspěšně zpracovali
                AiInput = "";
            }

            // 5. Tady je prostor pro další příkazy (reset, dth, atd.)
            if (commands.Any(c => c.Name.Equals("reset", StringComparison.OrdinalIgnoreCase)))
            {
                // TODO: Implementovat logiku pro reset (např. vymazání logů nebo resetování session)
                Logs.Add("🔄 Reset command detected (logic not implemented yet).");
            }
        }

    }
}
