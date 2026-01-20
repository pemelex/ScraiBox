using ScraiBox.Core;
using ScraiBox.Core.Interfaces.DTO;
using ScraiBox.Plugin.UC.Analysis;
using ScraiBox.Plugin.UC.Implementation;
using ScraiBox.Plugin.UC.System;

namespace ScraiBox.WinGui
{
    public partial class MainForm : Form
    {
        private string _projectPath = "";
        private InventoryService _inventoryService = new();
        private RoslynService _roslynService = new();
        private AdvancedRoslynService _advanceRoslynService = new();
        private CommandInterceptor _interceptor = new();

        public MainForm()
        {
            InitializeComponent();
            // Inicializace výchozích hodnot
            comboUseCase.SelectedIndex = 0;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                _projectPath = fbd.SelectedPath;
                lblPath.Text = _projectPath;
                Log("Project root linked successfully.");
                _ = GenerateJsonMap(); // Spustíme na pozadí
            }
        }

        // Handler pro tlačítko "Process & Copy" (příkazy typu scry)
        private async void btnProcess_Click(object sender, EventArgs e)
        {
            // Zamezíme vícenásobnému kliknutí během zpracování
            btnProcess.Enabled = false;
            try
            {
                await ProcessCommand();
            }
            finally
            {
                btnProcess.Enabled = true;
            }
        }

        // Handler pro spouštění Use Casů (DeepContextTracer atd.)
        private async void btnRunUseCase_Click(object sender, EventArgs e)
        {
            btnRunUseCase.Enabled = false;
            try
            {
                await ExecuteUseCase();
            }
            finally
            {
                btnRunUseCase.Enabled = true;
            }
        }

        private async Task ProcessCommand()
        {
            if (string.IsNullOrWhiteSpace(_projectPath))
            {
                Log("⚠️ Error: Select project folder first!");
                return;
            }

            string input = txtAiInput.Text;
            if (string.IsNullOrWhiteSpace(input)) return;

            var commands = _interceptor.InterceptAll(input);

            if (commands.Count == 0)
            {
                Log("⚠️ No valid command found. Check the format: <!cmd:name:param>");
                return;
            }

            var scryPaths = commands
                .Where(c => c.Name.Equals("scry", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Parameter)
                .ToList();

            if (scryPaths.Any())
            {
                var scryer = new ContextScryer(_projectPath);
                string content = scryer.ScryMultiple(scryPaths);

                // WinForms verze Clipboardu
                Clipboard.SetText(content);

                Log($"🚀 Success! Context for {scryPaths.Count} items is in your clipboard.");
                txtAiInput.Clear();
            }
        }

        private async Task ExecuteUseCase()
        {
            if (string.IsNullOrEmpty(_projectPath))
            {
                Log("⚠️ Select project folder first!");
                return;
            }

            var selected = comboUseCase.SelectedItem!.ToString();
            var context = new ScraiBoxContext
            {
                ProjectRootPath = _projectPath,
                RawInput = txtTarget.Text,
                Inventory = _inventoryService.BuildInventory(_projectPath)
            };

            try
            {
                ScraiBoxResult result = selected switch
                {
                    "SelfHydration" => await new SelfHydrationUseCase().ExecuteAsync(context),
                    "DeepContextTracer" => await new DeepContextTracerUseCase(_advanceRoslynService).ExecuteAsync(context),
                    "BlazorEdit" => await new BlazorComponentEditUseCase(_inventoryService, _roslynService).ExecuteAsync(context),
                    "MethodCallTree" => await new MethodCallTreeUseCase(_inventoryService, _roslynService).ExecuteAsync(context),
                    _ => throw new NotImplementedException()
                };

                Clipboard.SetText(result.OutputData);
                Log($"✅ {selected} data copied to clipboard!");
            }
            catch (Exception ex)
            {
                Log($"❌ ERROR: {ex.Message}");
                Log($"📍 SOURCE: {ex.TargetSite}");
                Log($"📜 STACK: {ex.StackTrace}");
            }
        }

        private async Task GenerateJsonMap()
        {
            if (string.IsNullOrEmpty(_projectPath)) return;

            try
            {
                // Použití tvého ProjectMapperu 
                var mapper = new ProjectMapper(_projectPath);
                string jsonMap = mapper.GenerateJsonMap();

                // Sestavení výstupu s instrukcemi pro AI 
                string fullOutput = $"# INSTRUCTIONS\n" +
                                   $"If you need full code of any file, reply with: <!cmd:scry:relative_path>\n\n" +
                                   $"# PROJECT JSON MAP\n{jsonMap}";

                // Uložení do project_map.md v kořenu projektu 
                string path = Path.Combine(_projectPath, "project_map.md");
                await File.WriteAllTextAsync(path, fullOutput);

                // WinForms Clipboard (běží v UI threadu, takže Invoke není třeba, pokud voláme z UI eventu)
                Clipboard.SetText(fullOutput);

                Log("✅ Project Map (JSON + Instructions) generated and copied to clipboard!");
            }
            catch (Exception ex)
            {
                Log($"❌ Mapper Error: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            // WinForms vyžaduje Invoke, pokud logujeme z jiného vlákna
            if (lstLogs.InvokeRequired)
            {
                lstLogs.Invoke(() => Log(message));
                return;
            }
            lstLogs.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }
    }
}
