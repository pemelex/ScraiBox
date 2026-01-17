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