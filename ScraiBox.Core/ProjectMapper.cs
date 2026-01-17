using System.Text.Json;
using System.Text.RegularExpressions;

namespace ScraiBox.Core;

// Data structures for JSON serialization
public class ProjectNode
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Folder"; // Folder or File
    public string Path { get; set; } = string.Empty;
    public List<ProjectNode> Children { get; set; } = new();
    public List<string> Signatures { get; set; } = new();
}

public class ProjectMapper
{
    private readonly string _rootPath;
    private readonly string[] _ignoredDirs = { ".git", ".vs", "bin", "obj", "node_modules" };
    private static readonly Regex MethodRegex = new(@"\b(public|protected|private|static|async)\s+[\w<>[\]]+\s+(?<methodName>\w+)\s*\(", RegexOptions.Compiled);

    public ProjectMapper(string rootPath) => _rootPath = rootPath;

    public string GenerateJsonMap()
    {
        var rootNode = ScanDirectory(new DirectoryInfo(_rootPath));

        // Using Indented for readability, though Minified saves tokens
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(rootNode, options);
    }

    private ProjectNode ScanDirectory(DirectoryInfo dirInfo)
    {
        var node = new ProjectNode
        {
            Name = dirInfo.Name,
            Type = "Folder",
            Path = Path.GetRelativePath(_rootPath, dirInfo.FullName)
        };

        foreach (var dir in dirInfo.GetDirectories().Where(d => !_ignoredDirs.Contains(d.Name)))
        {
            node.Children.Add(ScanDirectory(dir));
        }

        foreach (var file in dirInfo.GetFiles().Where(f => f.Extension == ".cs"))
        {
            var fileNode = new ProjectNode
            {
                Name = file.Name,
                Type = "File",
                Path = Path.GetRelativePath(_rootPath, file.FullName),
                Signatures = ExtractSignatures(file.FullName)
            };
            node.Children.Add(fileNode);
        }

        return node;
    }

    private List<string> ExtractSignatures(string filePath)
    {
        var signatures = new List<string>();
        var content = File.ReadAllText(filePath);
        var matches = MethodRegex.Matches(content);
        foreach (Match match in matches)
        {
            signatures.Add($"{match.Groups["methodName"].Value}()");
        }
        return signatures;
    }
}