using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using ScraiBox.Core.Interfaces.DTO;
using System.Xml.Linq;

namespace ScraiBox.Core
{
    public class AdvancedRoslynService
    {
        private AdhocWorkspace? _workspace;

        public AdvancedRoslynService()
        {
        }

        public void InitializeFromInventory(ProjectInventory inventory, string rootPath)
        {
            _workspace = new AdhocWorkspace();
            var solution = _workspace.CurrentSolution;

            var projectGroups = inventory.Files
                .Where(f => f.Type == FileType.CSharp)
                .GroupBy(f => f.ProjectName)
                .ToList();

            foreach (var projectGroup in projectGroups)
            {
                // Najdeme .csproj soubor pro tento projekt v inventáři
                var projectFile = inventory.Files.FirstOrDefault(f => f.ProjectName == projectGroup.Key && f.Type == FileType.Project);
                ProjectMetadata? metadata = null;

                if (projectFile != null)
                {
                    metadata = AnalyzeProjectFile(Path.Combine(rootPath, projectFile.RelativePath));
                }

                var projectId = ProjectId.CreateNewId();
                var projectInfo = ProjectInfo.Create(
                    projectId,
                    VersionStamp.Create(),
                    projectGroup.Key,
                    projectGroup.Key,
                    LanguageNames.CSharp);

                solution = solution.AddProject(projectInfo);

                // Aplikace referencí podle typu frameworku
                if (metadata != null && metadata.IsLegacy)
                {
                    solution = AddLegacyReferences(solution, projectId);
                }
                else
                {
                    solution = AddModernReferences(solution, projectId);
                }

                // Přidání .cs souborů
                foreach (var file in projectGroup)
                {
                    var sourceText = SourceText.From(File.ReadAllText(Path.Combine(rootPath, file.RelativePath)));
                    solution = solution.AddDocument(DocumentId.CreateNewId(projectId), file.Name, sourceText);
                }
            }

            _workspace.TryApplyChanges(solution);
        }

        private Solution AddLegacyReferences(Solution solution, ProjectId projectId)
        {
            string net48Path = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8";
            if (Directory.Exists(net48Path))
            {
                var libs = new[] { "mscorlib.dll", "System.dll", "System.Core.dll", "System.Data.dll" };
                foreach (var lib in libs)
                {
                    solution = solution.AddMetadataReference(projectId, MetadataReference.CreateFromFile(Path.Combine(net48Path, lib)));
                }
            }
            return solution;
        }

        public Solution AddModernReferences(Solution solution, ProjectId projectId)
        {
            // Moderní .NET 10 cesta
            var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
                .Split(Path.PathSeparator)
                .Select(path => MetadataReference.CreateFromFile(path))
                .Cast<MetadataReference>()
                .ToList();
            solution = solution.AddMetadataReferences(projectId, references);
            return solution;
        }

        public ProjectMetadata AnalyzeProjectFile(string csprojPath)
        {
            var metadata = new ProjectMetadata();
            var doc = XDocument.Load(csprojPath);
            XNamespace ns = doc.Root?.GetDefaultNamespace() ?? XNamespace.None;

            // 1. Detekce Frameworku
            var tfVersion = doc.DescendantNodes().OfType<XElement>()
                .FirstOrDefault(x => x.Name.LocalName == "TargetFrameworkVersion" || x.Name.LocalName == "TargetFramework")?.Value;

            metadata.TargetFramework = tfVersion ?? "unknown";
            metadata.IsLegacy = metadata.TargetFramework.StartsWith("v4.");

            // 2. Extrakce referencí (NuGet a přímé reference)
            var refs = doc.DescendantNodes().OfType<XElement>()
                .Where(x => x.Name.LocalName == "Reference" || x.Name.LocalName == "PackageReference")
                .Select(x => x.Attribute("Include")?.Value ?? x.Attribute("Update")?.Value)
                .Where(v => v != null);

            metadata.References.AddRange(refs!);
            return metadata;
        }

        public async Task<(string Name, string? SourceCode, bool IsLocal)?> GetMethodDetailsByNameAsync(string className, string methodName)
        {
            if (_workspace == null) return null;

            // Prohledáme všechny projekty v naší solution
            foreach (var project in _workspace.CurrentSolution.Projects)
            {
                var compilation = await project.GetCompilationAsync();
                if (compilation == null) continue;

                // Najdeme třídu podle názvu
                var typeSymbol = compilation.GetSymbolsWithName(className, SymbolFilter.Type)
                                            .OfType<INamedTypeSymbol>()
                                            .FirstOrDefault();

                if (typeSymbol != null)
                {
                    // Najdeme metodu v této třídě
                    var methodSymbol = typeSymbol.GetMembers(methodName)
                                                 .OfType<IMethodSymbol>()
                                                 .FirstOrDefault();

                    if (methodSymbol != null)
                    {
                        // Teď už máme IMethodSymbol, tak zavoláme tu existující logiku
                        return await GetMethodDetailsAsync(methodSymbol);
                    }
                }
            }

            return null;
        }

        public async Task<(string Name, string? SourceCode, bool IsLocal)> GetMethodDetailsAsync(IMethodSymbol symbol)
        {
            string fullName = $"{symbol.ContainingType.Name}.{symbol.Name}";

            // DeclaringSyntaxReferences obsahuje odkazy na zdrojový kód v naší solution
            var syntaxRef = symbol.DeclaringSyntaxReferences.FirstOrDefault();

            if (syntaxRef == null)
            {
                // Nemáme zdroják (je to .NET Framework, NuGet, nebo jiná DLL)
                return (fullName, null, false);
            }

            var node = await syntaxRef.GetSyntaxAsync();
            // Vrátíme plný kód metody a označíme jako LOCAL
            return (fullName, node.ToFullString(), true);
        }

        public async Task<SemanticModel?> GetSemanticModelForClassAsync(string className)
        {
            if (_workspace == null) throw new Exception("Workspace not initialized!");

            var document = _workspace.CurrentSolution.Projects
                .SelectMany(p => p.Documents)
                .FirstOrDefault(d => d.Name.Equals($"{className}.cs", StringComparison.OrdinalIgnoreCase));

            if (document == null) return null;
            return await document.GetSemanticModelAsync();
        }

        public async Task<HashSet<string>> GetDeepMethodCallsAsync(string className, string methodName)
        {
            if (_workspace == null) throw new InvalidOperationException("Workspace not initialized!");
            var calls = new HashSet<string>();
            var solution = _workspace.CurrentSolution;

            // 1. Find the starting method symbol
            IMethodSymbol? targetMethodSymbol = null;
            foreach (var project in solution.Projects)
            {
                var compilation = await project.GetCompilationAsync();
                if (compilation == null) continue;

                // Efficiently find types by name instead of iterating all documents
                var typeSymbol = compilation.GetSymbolsWithName(className, SymbolFilter.Type).OfType<INamedTypeSymbol>().FirstOrDefault();
                if (typeSymbol != null)
                {
                    targetMethodSymbol = typeSymbol.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault();
                    if (targetMethodSymbol != null) break;
                }
            }

            if (targetMethodSymbol == null) return calls;

            // 2. Trace calls inside the method body
            foreach (var reference in targetMethodSymbol.DeclaringSyntaxReferences)
            {
                var node = await reference.GetSyntaxAsync();
                var model = solution.GetDocument(node.SyntaxTree)?.GetSemanticModelAsync().Result; // Simple access for brevity
                if (model == null) continue;

                var invocations = node.DescendantNodes().OfType<InvocationExpressionSyntax>();

                foreach (var invocation in invocations)
                {
                    var symbolInfo = model.GetSymbolInfo(invocation);
                    var calledMethod = symbolInfo.Symbol as IMethodSymbol;

                    if (calledMethod != null)
                    {
                        // Logic for Interfaces: If it's an interface, we might want to find implementations
                        if (calledMethod.ContainingType.TypeKind == TypeKind.Interface)
                        {
                            // For now, we log the interface call, 
                            // but we can use SymbolFinder.FindImplementationsAsync(calledMethod, solution) later
                            calls.Add($"[Interface]{calledMethod.ContainingType.Name}.{calledMethod.Name}");
                        }
                        else
                        {
                            calls.Add($"{calledMethod.ContainingType.Name}.{calledMethod.Name}");
                        }
                    }
                }
            }

            return calls;
        }

        /// <summary>
        /// Creates a compilation for a single file to allow semantic analysis.
        /// In a full ScraiBox implementation, this would be part of a Project/Solution-wide compilation.
        /// </summary>
        private async Task<SemanticModel> GetSemanticModelAsync(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("ScraiBoxAnalysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                // Přidáme základní reference, aby Roslyn mohl typovat
                .AddSyntaxTrees(tree);

            return compilation.GetSemanticModel(tree);
        }

        /*public async Task<HashSet<string>> GetMethodCallsAsync(string code, string className, string methodName)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();
            var model = await GetSemanticModelAsync(code);
            var calls = new HashSet<string>();

            var methodDecl = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text == methodName &&
                                    (m.Parent as ClassDeclarationSyntax)?.Identifier.Text == className);

            if (methodDecl == null) return calls;

            var invocations = methodDecl.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var invocation in invocations)
            {
                // Tady se děje to kouzlo: model.GetSymbolInfo nám řekne přesně, co se volá
                var symbolInfo = model.GetSymbolInfo(invocation);
                var methodSymbol = symbolInfo.Symbol as IMethodSymbol;

                if (methodSymbol != null)
                {
                    // Získáme plný název: "Třída.Metoda"
                    string fullCall = $"{methodSymbol.ContainingType.Name}.{methodSymbol.Name}";
                    calls.Add(fullCall);
                }
            }

            return calls;
        }*/

        // V InventoryService.cs přidej metodu pro sběr cest k DLL
        public List<string> GetExternalReferences(string projectPath)
        {
            var refs = new List<string>();
            // Pro .NET 4.8 často existuje packages.config ve složce projektu
            var configPath = Path.Combine(Path.GetDirectoryName(projectPath), "packages.config");

            if (File.Exists(configPath))
            {
                var doc = XDocument.Load(configPath);
                // Tady bychom parsovali ID balíčků a hledali je v ../packages/ složce
                // Pro ScraiBox to ale zatím uděláme jednodušeji - budeme skenovat bin\Debug
            }

            return refs;
        }

        public async Task<string> GetMethodSourceCodeAsync(string code, string className, string methodName)
        {
            // Syntaktická část zůstává podobná, ale je čistší
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();

            var method = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text == methodName &&
                                    (m.Parent as ClassDeclarationSyntax)?.Identifier.Text == className);

            return method?.ToFullString() ?? string.Empty;
        }
    }
}
