using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Analyzers
{
    /// <summary>
    /// Extension methods for syntax types.
    /// </summary>
    public static class SyntaxExtensions
    {
        /// <summary>
        /// Tries to find a <see cref="MethodDeclarationSyntax"/> near <paramref name="tokenPosition"/>.
        /// </summary>
        /// <param name="root">The syntax node root.</param>
        /// <param name="tokenPosition">The token position.</param>
        /// <returns><see cref="MethodDeclarationSyntax"/> if found, or null.</returns>
        public static MethodDeclarationSyntax TryGetMethodDeclaratioNearPosition(this SyntaxNode root, int tokenPosition)
        {
            var token = root.FindToken(tokenPosition);

            foreach (var node in token.Parent.AncestorsAndSelf())
            {
                Type nodeType = node.GetType();
                if (nodeType == typeof(MethodDeclarationSyntax))
                {
                    return (MethodDeclarationSyntax)node;
                }
                else if (nodeType == typeof(BlockSyntax))
                {
                    return null;
                }
            }

            return null;
        }
    }
}