# Project Context: ScraiBox
# Generated: 2026-01-17 19:58:10
---
## File: current_context.md
```csharp
# Project Context: ScraiBox
# Generated: 2026-01-17 19:54:07
---
## File: README.md
```csharp
# ScraiBox
ScraiBox: A developer's scrying glass for AI-driven development. Tools to bridge the gap between .NET codebase and AI.

```

## File: ScraiBox.Console\Program.cs
```csharp
using ScraiBox.Core;

Console.WriteLine("--- ScraiBox: Developer's Scrying Glass ---");
Console.WriteLine($"Starting at: {DateTime.Now}");

// Získáme cestu k rootu solution (o dvě úrovně výš než je bin/Debug/...)
string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

Console.WriteLine($"Scanning directory: {projectRoot}");

var scryer = new ContextScryer(projectRoot);
var output = scryer.Scry();

// Pro začátek to jen vypíšeme do konzole, ale můžeme to i uložit
Console.WriteLine("\n--- SCAN COMPLETED ---");
Console.WriteLine(output);

// Tip: Můžeme to rovnou uložit do souboru, který pak jen 'hodíš' do ChatGPT/Claude
string outputPath = Path.Combine(projectRoot, "current_context.md");
File.WriteAllText(outputPath, output);

Console.WriteLine($"\nContext saved to: {outputPath}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```

## File: ScraiBox.Core\ContextScryer.cs
```csharp
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

    public string Scry()
    {
        var output = new System.Text.StringBuilder();
        output.AppendLine($"# Project Context: {Path.GetFileName(_rootPath)}");
        output.AppendLine($"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        output.AppendLine("---");

        foreach (var file in Directory.EnumerateFiles(_rootPath, "*.*", SearchOption.AllDirectories))
        {
            var folderName = Path.GetDirectoryName(file);
            if (_excludedFolders.Any(f => file.Contains($"{Path.DirectorySeparatorChar}{f}{Path.DirectorySeparatorChar}")))
                continue;

            if (!_allowedExtensions.Contains(Path.GetExtension(file)))
                continue;

            output.AppendLine($"## File: {Path.GetRelativePath(_rootPath, file)}");
            output.AppendLine("```csharp");
            output.AppendLine(File.ReadAllText(file));
            output.AppendLine("```");
            output.AppendLine();
        }

        return output.ToString();
    }
}
```


```

## File: README.md
```csharp
# ScraiBox
ScraiBox: A developer's scrying glass for AI-driven development. Tools to bridge the gap between .NET codebase and AI.

```

## File: ScraiBox.Console\Program.cs
```csharp
using ScraiBox.Core;

Console.WriteLine("--- ScraiBox: Developer's Scrying Glass ---");
Console.WriteLine($"Starting at: {DateTime.Now}");

// Získáme cestu k rootu solution (o dvě úrovně výš než je bin/Debug/...)
string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

Console.WriteLine($"Scanning directory: {projectRoot}");

var scryer = new ContextScryer(projectRoot);
var output = scryer.Scry();

// Pro začátek to jen vypíšeme do konzole, ale můžeme to i uložit
Console.WriteLine("\n--- SCAN COMPLETED ---");
Console.WriteLine(output);

// Tip: Můžeme to rovnou uložit do souboru, který pak jen 'hodíš' do ChatGPT/Claude
string outputPath = Path.Combine(projectRoot, "current_context.md");
File.WriteAllText(outputPath, output);

Console.WriteLine($"\nContext saved to: {outputPath}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```

## File: ScraiBox.Core\ContextScryer.cs
```csharp
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

    public string Scry()
    {
        var output = new System.Text.StringBuilder();
        output.AppendLine($"# Project Context: {Path.GetFileName(_rootPath)}");
        output.AppendLine($"# Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        output.AppendLine("---");

        foreach (var file in Directory.EnumerateFiles(_rootPath, "*.*", SearchOption.AllDirectories))
        {
            var folderName = Path.GetDirectoryName(file);
            if (_excludedFolders.Any(f => file.Contains($"{Path.DirectorySeparatorChar}{f}{Path.DirectorySeparatorChar}")))
                continue;

            if (!_allowedExtensions.Contains(Path.GetExtension(file)))
                continue;

            output.AppendLine($"## File: {Path.GetRelativePath(_rootPath, file)}");
            output.AppendLine("```csharp");
            output.AppendLine(File.ReadAllText(file));
            output.AppendLine("```");
            output.AppendLine();
        }

        return output.ToString();
    }
}
```

