using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.DocumentAnalyzers
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
            return TryGetSyntaxNodeNearPosition<MethodDeclarationSyntax>(root, tokenPosition, new[] { typeof(ClassDeclarationSyntax) });
        }

        /// <summary>
        /// Tries to find a <typeparamref name="TSyntax"/> type exctly under <paramref name="tokenPosition"/>.
        /// </summary>
        /// <param name="root">The syntax node root.</param>
        /// <param name="tokenPosition">The token position.</param>
        /// <returns>An instance of <typeparamref name="TSyntax"/> if found, or null.</returns>
        public static TSyntax TryGetSyntaxNodeUnderPosition<TSyntax>(this SyntaxNode root, int tokenPosition) where TSyntax : SyntaxNode
        {
            var token = root.FindToken(tokenPosition);
            var syntaxNode = token.Parent;

            if (syntaxNode is TSyntax node)
            {
                return node;
            }

            return null;
        }

        /// <summary>
        /// Tries to find a <typeparamref name="TSyntax"/> type near <paramref name="tokenPosition"/>.
        /// </summary>
        /// <param name="root">The syntax node root.</param>
        /// <param name="tokenPosition">The token position.</param>
        /// <param name="breakoutTypes">One or more syntax types that if found will break the search. Can be null or empty for an exaustive search.</param>
        /// <returns>An instance of <typeparamref name="TSyntax"/> if found, or null.</returns>
        public static TSyntax TryGetSyntaxNodeNearPosition<TSyntax>(this SyntaxNode root, int tokenPosition, IEnumerable<Type> breakoutTypes = null) where TSyntax : SyntaxNode
        {
            breakoutTypes = breakoutTypes ?? Enumerable.Empty<Type>();
            var token = root.FindToken(tokenPosition);

            foreach (var node in token.Parent.AncestorsAndSelf())
            {
                Type nodeType = node.GetType();
                if (nodeType == typeof(TSyntax))
                {
                    return (TSyntax)node;
                }
                else if (breakoutTypes.Contains(nodeType))
                {
                    return null;
                }
            }

            return null;
        }
    }
}