using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core.Interfaces.DTO
{
    public class ScraiBoxContext
    {
        // Základní vstup od uživatele/AI
        public List<InterceptedCommand> Commands { get; init; } = new();
        public string RawInput { get; init; } = string.Empty;

        // Cesty a prostředí
        public string ProjectRootPath { get; init; } = string.Empty;
        public string WorkingDirectory { get; init; } = string.Empty;

        public ProjectInventory? Inventory { get; set; }

        // Služby (aby UC mohl "konat")
        //public IToolRegistry ToolRegistry { get; init; } = null!;

        // Progres a logger pro UI (WASM/MAUI)
        public IProgress<string>? ProgressReporter { get; init; }
        public CancellationToken CancellationToken { get; init; } = CancellationToken.None;

        // Dictionary pro specifické potřeby různých UC (extensibilita)
        public Dictionary<string, object> ExtraData { get; init; } = new();
    }
}
