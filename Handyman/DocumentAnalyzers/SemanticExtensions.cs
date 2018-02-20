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
    }
}
