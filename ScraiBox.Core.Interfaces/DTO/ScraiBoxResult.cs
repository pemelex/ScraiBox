using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core.Interfaces.DTO
{

    public class ScraiBoxResult
    {
        public bool IsSuccess { get; init; }
        public string Message { get; init; } = string.Empty;
        public List<string> AffectedFiles { get; init; } = new();
        public string? OutputData { get; init; } // Např. JSON mapa projektu
        public Exception? Error { get; init; }
    }
}
