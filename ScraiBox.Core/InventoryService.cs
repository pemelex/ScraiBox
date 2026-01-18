using ScraiBox.Core.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core
{
    public class InventoryService
    {
        public ProjectInventory BuildInventory(string rootPath)
        {
            var inventory = new ProjectInventory { LastUpdated = DateTime.Now };
            var rootDir = new DirectoryInfo(rootPath);

            var extensions = new Dictionary<string, FileType>
        {
            { ".cs", FileType.CSharp },
            { ".razor", FileType.Razor },
            { ".json", FileType.Json },
            { ".csproj", FileType.Project }
        };

            var files = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories)
                .Where(f => !f.Contains("\\bin\\") && !f.Contains("\\obj\\") && !f.Contains("\\.git\\"));

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file).ToLower();
                if (extensions.TryGetValue(ext, out var type))
                {
                    inventory.Files.Add(new FileEntry(
                        Path.GetFileName(file),
                        Path.GetRelativePath(rootPath, file),
                        type,
                        GetProjectName(file, rootPath)
                    ));
                }
            }
            return inventory;
        }

        public IEnumerable<FileEntry> Search(ProjectInventory inventory, string term)
        {
            return inventory.Files
                .Where(f => f.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Take(20);
        }

        private string GetProjectName(string filePath, string rootPath)
        {
            // Najde nejbližší složku, která obsahuje .csproj směrem nahoru k rootu
            var directory = Path.GetDirectoryName(filePath);
            while (directory != null && directory.Length >= rootPath.Length)
            {
                var projectFile = Directory.EnumerateFiles(directory, "*.csproj").FirstOrDefault();
                if (projectFile != null)
                    return Path.GetFileNameWithoutExtension(projectFile);

                directory = Path.GetDirectoryName(directory);
            }
            return "Unknown";
        }
    }
}
