using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core
{
    public class TraceWriter : IDisposable
    {
        private readonly StringBuilder _sb = new();
        private readonly StreamWriter? _fileWriter;

        public TraceWriter(string projectName)
        {
            try
            {
                // Cesta: <Složka ScraiBoxu>\Logs\<ProjectName>\trace_aktuální_datum.txt
                string baseDir = Path.Combine(AppContext.BaseDirectory, "Logs", projectName);
                if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);

                string fileName = $"trace_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                _fileWriter = new StreamWriter(Path.Combine(baseDir, fileName), append: true) { AutoFlush = true };
            }
            catch
            {
                // Pokud selže soubor, budeme aspoň držet StringBuilder
            }
        }

        public void AppendLine(string text)
        {
            _sb.AppendLine(text);
            _fileWriter?.WriteLine(text);
        }

        public override string ToString() => _sb.ToString();

        public void Dispose()
        {
            _fileWriter?.Dispose();
        }
    }
}
