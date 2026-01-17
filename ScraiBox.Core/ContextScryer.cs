namespace ScraiBox.Core;

public class ContextScryer
{
    private readonly string _rootPath;
    private readonly string[] _excludedFolders = { "bin", "obj", ".git", ".vs", "node_modules" };
    private readonly string[] _allowedExtensions = { ".cs", ".txt", ".md", ".json", ".yaml" };

    public ContextScryer(string rootPath)
    {
        _rootPath = rootPath;
    }

    public string Scry(string subPath = "")
    {
        var output = new System.Text.StringBuilder();

        // Combine root with the requested subpath (e.g. "ScraiBox.Core/Logic.cs")
        string fullPath = Path.Combine(_rootPath, subPath);

        output.AppendLine($"# Project Context: {Path.GetFileName(_rootPath)}");
        if (!string.IsNullOrEmpty(subPath))
        {
            output.AppendLine($"# Scoped Path: {subPath}");
        }
        output.AppendLine($"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        output.AppendLine("---");

        if (File.Exists(fullPath))
        {
            // Case 1: The parameter is a single file
            AppendFileContent(fullPath, output);
        }
        else if (Directory.Exists(fullPath))
        {
            // Case 2: The parameter is a folder (scans everything inside)
            foreach (var file in Directory.EnumerateFiles(fullPath, "*.*", SearchOption.AllDirectories))
            {
                if (_excludedFolders.Any(f => file.Contains($"{Path.DirectorySeparatorChar}{f}{Path.DirectorySeparatorChar}")))
                    continue;

                if (!_allowedExtensions.Contains(Path.GetExtension(file)))
                    continue;

                AppendFileContent(file, output);
            }
        }
        else
        {
            output.AppendLine($"⚠️ Path not found: {subPath}");
        }

        return output.ToString();
    }

    public string ScryMultiple(IEnumerable<string> subPaths)
    {
        var output = new System.Text.StringBuilder();

        output.AppendLine($"# Multi-File Context Scry");
        output.AppendLine($"# Root: {Path.GetFileName(_rootPath)}");
        output.AppendLine($"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        output.AppendLine("---");

        foreach (var path in subPaths)
        {
            string fullPath = Path.Combine(_rootPath, path);

            if (File.Exists(fullPath))
            {
                AppendFileContent(fullPath, output);
            }
            else if (Directory.Exists(fullPath))
            {
                // Pokud by náhodou jeden z parametrů byl složka
                output.AppendLine($"### [Folder: {path}]");
                foreach (var file in Directory.EnumerateFiles(fullPath, "*.*", SearchOption.AllDirectories))
                {
                    if (IsExcluded(file)) continue;
                    AppendFileContent(file, output);
                }
            }
            else
            {
                output.AppendLine($"⚠️ Path not found: {path}");
            }
        }

        return output.ToString();
    }

    // Pomocná metoda pro validaci (vytáhnuto z původní logiky)
    private bool IsExcluded(string filePath)
    {
        if (_excludedFolders.Any(f => filePath.Contains($"{Path.DirectorySeparatorChar}{f}{Path.DirectorySeparatorChar}")))
            return true;

        return !_allowedExtensions.Contains(Path.GetExtension(filePath));
    }

    private void AppendFileContent(string filePath, System.Text.StringBuilder sb)
    {
        sb.AppendLine($"## File: {Path.GetRelativePath(_rootPath, filePath)}");
        sb.AppendLine("```csharp");
        sb.AppendLine(File.ReadAllText(filePath));
        sb.AppendLine("```");
        sb.AppendLine();
    }
}