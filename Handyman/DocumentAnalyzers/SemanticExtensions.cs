using Microsoft.CodeAnalysis;

namespace Handyman.DocumentAnalyzers
{
    /// <summary>
    /// Method extensions for the semantic model.
    /// </summary>
    public static class SemanticExtensions
    {
        /// <summary>
        /// Checks if <see cref="type"/> is of type <paramref name="baseType"/> or is derived from it.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="baseType">The expected base type.</param>
        /// <returns>true if <see cref="type"/> is of type <paramref name="baseType"/> or is derived from it; false otherwise.</returns>
        public static bool IsDerivedFrom(this ITypeSymbol type, ITypeSymbol baseType)
        {
            while (type != null && !type.Equals(baseType))
            {
                type = type.BaseType;
            }

            return type != null;
        }

        /// <summary>
        /// Gets the fully qualified name for <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the name for.</param>
        /// <returns>The fully qualified name for <paramref name="type"/></returns>
        public static string GetFullyQualifiedName(this ITypeSymbol type)
        {
            var symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
            return type.ToDisplayString(symbolDisplayFormat);
        }

        /// <summary>
        /// Checks wherther <paramref name="compareTo"/> is the same type as <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to compare.</param>
        /// <param name="compareTo">The type to compare to.</param>
        /// <returns>A value indicating whether <paramref name="compareTo"/> is the same type as <paramref name="type"/>.</returns>
        public static bool SameAsType(this ITypeSymbol type, ITypeSymbol compareTo)
        {
            // TODO consider implementing this instead: https://stackoverflow.com/questions/34243031/reliably-compare-type-symbols-itypesymbol-with-roslyn
            return type.GetFullyQualifiedName().Equals(compareTo.GetFullyQualifiedName(), System.StringComparison.Ordinal);
        }
    }
}
