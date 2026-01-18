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
        private bool IsBasicType(string typeName) =>
            typeName is "string" or "int" or "bool" or "Task" or "void" or "DateTime" or "decimal" or "long" or "float" or "double" or "Guid" or "object" or "var";
    }
}
