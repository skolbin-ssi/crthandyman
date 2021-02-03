﻿using System;
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
        private readonly AnalysisContext context;

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
        public RequestHandlerDefinition TryGetRequestHandlerFromSyntaxTree(TextSpan textSpan, CancellationToken cancellationToken = default)
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
                    var declaringReference = member.DeclaringSyntaxReferences.FirstOrDefault();

                    IEnumerable<ITypeSymbol> declaredSupportedRequestTypes;

                    // WARNING: the declaring member could be in a different syntax tree than the analysis context
                    // this will happen when the property is not declared in the class itself, but on a base class, for example
                    // we must not use the semantic model or syntax tree from context here
                    if (declaringReference.SyntaxTree != this.context.SyntaxRoot.SyntaxTree)
                    {

                        // supporting handcrafted scenarios
                        if (classDeclaration?.BaseType?.ConstructedFrom?.ToDisplayString() == "Microsoft.Dynamics.Commerce.Runtime.SingleRequestHandler<TRequest, TResponse>")
                        {
                            declaredSupportedRequestTypes = new[] { classDeclaration.BaseType.TypeArguments.First() };
                        }
                        else
                        {
                            // scenario not supported
                            declaredSupportedRequestTypes = Enumerable.Empty<ITypeSymbol>();
                        }
                    }
                    else
                    {
                        var declaringNode = this.context.SyntaxRoot.FindNode(declaringReference.Span);
                        declaredSupportedRequestTypes = SearchDescendantNodesForRequestSymbols(declaringNode, this.context, cancellationToken);
                    }

                    return new RequestHandlerDefinition(classDeclaration, this.context.Document, declaredSupportedRequestTypes);
                }
            }

            return null;
        }

        public static IEnumerable<TypeLocation> FindRequestImplementationLocations(
            RequestHandlerDefinition requestHandlerDefinition,
            AnalysisContext context,
            CancellationToken cancellationToken = default)
        {
            var executeInterfaceMethod = context.CommerceRuntimeReference.IRequestHandlerTypeSymbol.GetMembers("Execute").FirstOrDefault();
            var executeMethod = requestHandlerDefinition.ClassType.FindImplementationForInterfaceMember(executeInterfaceMethod);

            var methodDeclarationSyntax = context.SyntaxRoot.FindNode(executeMethod.DeclaringSyntaxReferences.FirstOrDefault().Span);

            return methodDeclarationSyntax.DescendantNodes()
                .Select(n => new Tuple<SyntaxNode, ITypeSymbol>(n, GetRequestTypeFromNode(n, context.SemanticModel, context.CommerceRuntimeReference, cancellationToken)))
                .Where(n => n.Item2 != null)
                .Select(n => new TypeLocation() { Location = n.Item1.GetLocation(), SyntaxNode = n.Item1, TypeSymbol = n.Item2 });
        }

        private static ITypeSymbol GetRequestTypeFromNode(SyntaxNode n, SemanticModel model, CommerceRuntimeReference reference, CancellationToken cancellationToken = default)
        {
            if (n is IdentifierNameSyntax node)
            {
                var typeInfo = model.GetTypeInfo(node, cancellationToken);
                if (typeInfo.Type != null && typeInfo.Type.IsDerivedFrom(reference.RequestTypeSymbol))
                {
                    // this is a syntax node that points to a request type
                    return typeInfo.Type;
                }
            }

            return null;
        }

        private static IEnumerable<ITypeSymbol> SearchDescendantNodesForRequestSymbols(SyntaxNode root, AnalysisContext context, CancellationToken cancellationToken)
        {
            // TODO rewrite this in visitor pattern
            ReturnStatementSyntax returnSyntax = null;
            bool foundAny = false;

            foreach (var node in root.DescendantNodes())
            {
                if (node is ReturnStatementSyntax returnSyntaxNode)
                {
                    returnSyntax = returnSyntaxNode;
                }
                else
                {
                    var requestSymbol = GetRequestTypeFromNode(node, context.SemanticModel, context.CommerceRuntimeReference, cancellationToken);
                    if (requestSymbol != null)
                    {
                        foundAny = true;
                        yield return requestSymbol;
                    }
                }
            }

            if (!foundAny && returnSyntax != null)
            {
                // if we didn't find anything, consider return syntax node to be returning a variable and see if it points to some static value
                var firstDesc = returnSyntax.DescendantNodes().First();
                if (firstDesc is IdentifierNameSyntax identifierNode)
                {
                    var symbol = context.SemanticModel.GetSymbolInfo(identifierNode, cancellationToken);
                    var reference = symbol.Symbol?.DeclaringSyntaxReferences.FirstOrDefault();
                    if (reference != null && reference.SyntaxTree == context.SyntaxRoot.SyntaxTree)
                    {
                        var referenceNode = context.SyntaxRoot.FindNode(reference.Span);
                        foreach (var r in SearchDescendantNodesForRequestSymbols(referenceNode, context, cancellationToken))
                        {
                            yield return r;
                        }
                    }
                }
            }
        }
    }
}
