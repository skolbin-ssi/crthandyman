using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;

namespace Handyman.DocumentAnalyzers
{
    public class RequestHandlerAnalyzer
    {
        private AnalysisContext context;

        public RequestHandlerAnalyzer(AnalysisContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public RequestHandlerMethodDefinition TryGetHandlerDefinition(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var semanticModel = context.SemanticModel;

            if (methodDeclarationSyntax != null)
            {
                var methodSymbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
                return RequestHandlerMethodDefinition.TryParse(methodSymbol, context.CommerceRuntimeReference);
            }            

            return null;
        }

        /// <summary>
        /// Walks up the <paramref name="textSpan"/> and tries to find a class that implements a request handler.
        /// </summary>
        /// <param name="textSpan">A location in a <see cref="SyntaxTree"/> that is assumed to be a request handler class or within one.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="reference">The commerce runtime reference.</param>
        /// <returns>An instance of <see cref="RequestHandlerMethodDefinition"/> or null if not a request handler.</returns>
        public RequestHandlerDefinition TryGetRequestHandlerFromSyntaxTree(TextSpan textSpan, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            ClassDeclarationSyntax classNode = null;

            foreach (var node in this.context.SyntaxRoot.FindNode(textSpan)?.AncestorsAndSelf())
            {
                if (node is ClassDeclarationSyntax classNodeInstance)
                {
                    classNode = classNodeInstance;
                    break;
                }
                else if (node is MethodDeclarationSyntax methodNode)
                {
                    methods.Add(methodNode);
                }
            }

            if (classNode != null)
            {
                // found a class, see if it implements request handler interface
                var classDeclaration = this.context.SemanticModel.GetDeclaredSymbol(classNode, cancellationToken) as ITypeSymbol;
                bool isRequestHandler = classDeclaration.AllInterfaces.Contains(this.context.CommerceRuntimeReference.IRequestHandlerTypeSymbol);

                var supportedRequestHandlerMember = this.context.CommerceRuntimeReference.IRequestHandlerTypeSymbol.GetMembers("SupportedRequestTypes").FirstOrDefault();

                if (isRequestHandler)
                {                    
                    // now we can check what requests it implements
                    var member = classDeclaration.FindImplementationForInterfaceMember(supportedRequestHandlerMember);

                    // get the syntax node that declares them
                    var declaringNode = this.context.SyntaxRoot.FindNode(member.DeclaringSyntaxReferences.FirstOrDefault().Span);
                    var supportedRequestTypes = declaringNode.DescendantNodes()
                        .Select(n =>
                        {
                            if (n is IdentifierNameSyntax node)
                            {
                                var typeInfo = this.context.SemanticModel.GetTypeInfo(node, cancellationToken);
                                if (typeInfo.Type != null && typeInfo.Type.IsDerivedFrom(this.context.CommerceRuntimeReference.RequestTypeSymbol))
                                {
                                    // this is a request that this handle implements
                                    return typeInfo.Type;
                                }
                            }

                            return null;
                        })
                        .Where(n => n != null);

                    return new RequestHandlerDefinition(classDeclaration, this.context.Document, supportedRequestTypes);
                }
            }

            return null;
        }
    }
}
