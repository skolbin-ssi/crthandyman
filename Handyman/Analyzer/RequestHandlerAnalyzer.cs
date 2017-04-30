using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Analyzer
{
    public class RequestHandlerAnalyzer
    {
        private Document document;

        public RequestHandlerAnalyzer(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
        }

        public async Task<RequestHandlerDefinition> TryGetHandlerDefinition(int tokenPosition, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var syntaxTree = await semanticModel.SyntaxTree.GetRootAsync(cancellationToken);
            var token = syntaxTree.FindToken(tokenPosition);

            foreach (var node in token.Parent.AncestorsAndSelf())
            {
                Type nodeType = node.GetType();
                if (nodeType == typeof(MethodDeclarationSyntax))
                {
                    var methodSymbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(node);
                    return RequestHandlerDefinition.TryParse(methodSymbol);
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
