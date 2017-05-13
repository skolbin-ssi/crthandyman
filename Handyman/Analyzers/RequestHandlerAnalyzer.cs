using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Analyzers
{
    public class RequestHandlerAnalyzer
    {
        private AnalysisContext context;

        public RequestHandlerAnalyzer(AnalysisContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public RequestHandlerDefinition TryGetHandlerDefinition(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var semanticModel = context.SemanticModel;

            if (methodDeclarationSyntax != null)
            {
                var methodSymbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
                return RequestHandlerDefinition.TryParse(methodSymbol);
            }            

            return null;
        }
    }
}
