using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScraiBox.Core
{
    public class RoslynService
    {
        /// <summary>
        /// Analyzes the source code and returns a set of unique custom type names (DTOs, Models, etc.)
        /// </summary>
        public async Task<HashSet<string>> GetUsedTypeNamesAsync(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();
            var usedTypes = new HashSet<string>();

            // Extract type nodes from properties and parameters
            var propertyTypes = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().Select(p => p.Type);
            var parameterTypes = root.DescendantNodes().OfType<ParameterSyntax>().Select(p => p.Type);

            foreach (var typeSyntax in propertyTypes.Concat(parameterTypes))
            {
                if (typeSyntax == null) continue;

                foreach (var extractedType in ExtractActualTypes(typeSyntax))
                {
                    // Filter out primitive types and common .NET types
                    if (!IsBasicType(extractedType))
                        usedTypes.Add(extractedType);
                }
            }

            return usedTypes;
        }

        /// <summary>
        /// Recursively extracts type names from complex TypeSyntax (generics, arrays, qualified names)
        /// </summary>
        private IEnumerable<string> ExtractActualTypes(TypeSyntax typeSyntax)
        {
            switch (typeSyntax)
            {
                // Must be BEFORE SimpleNameSyntax because GenericNameSyntax inherits from it
                case GenericNameSyntax genericName:
                    yield return genericName.Identifier.Text;
                    foreach (var argument in genericName.TypeArgumentList.Arguments)
                    {
                        foreach (var nestedType in ExtractActualTypes(argument))
                            yield return nestedType;
                    }
                    break;

                case SimpleNameSyntax simpleName:
                    yield return simpleName.Identifier.Text;
                    break;

                case ArrayTypeSyntax arrayType:
                    foreach (var nestedType in ExtractActualTypes(arrayType.ElementType))
                        yield return nestedType;
                    break;

                case QualifiedNameSyntax qualifiedName:
                    yield return qualifiedName.Right.Identifier.Text;
                    foreach (var nestedType in ExtractActualTypes(qualifiedName.Left))
                        yield return nestedType;
                    break;
            }
        }

        /// <summary>
        /// Finds a method within a specific class and extracts its calls.
        /// </summary>
        public async Task<HashSet<string>> GetMethodCallsAsync(string code, string className, string methodName)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();
            var calls = new HashSet<string>();

            // 1. Find the class declaration first
            var classDecl = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text == className);

            if (classDecl == null) return calls;

            // 2. Find the method within that class
            var methodDecl = classDecl.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text == methodName);

            if (methodDecl == null) return calls;

            // 3. Extract invocations
            var invocations = methodDecl.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invocation in invocations)
            {
                calls.Add(invocation.Expression.ToString());
                // Note: Full string representation helps us see "service.DoSomething"
            }

            return calls;
        }

        public async Task<string> GetMethodSourceCodeAsync(string code, string className, string methodName)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();

            var method = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(c => c.Identifier.Text == className)
                .SelectMany(c => c.DescendantNodes().OfType<MethodDeclarationSyntax>())
                .FirstOrDefault(m => m.Identifier.Text == methodName);

            return method?.ToFullString() ?? "";
        }

        private bool IsBasicType(string typeName) =>
            typeName is "string" or "int" or "bool" or "Task" or "void" or "DateTime" or "decimal" or "long" or "float" or "double" or "Guid" or "object" or "var";
    }
}
