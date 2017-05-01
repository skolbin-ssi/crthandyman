using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Handyman.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Types
{
    /// <summary>
    /// Represents the implementation of a request handler.
    /// </summary>
    public class RequestHandlerDefinition
    {
        private IMethodSymbol requestHandlerMethod;

        public RequestHandlerDefinition(RequestType requestType, ResponseType responseType, IMethodSymbol requestHandlerMethod)
        {
            this.RequestType = requestType;
            this.ResponseType = responseType;
            this.requestHandlerMethod = requestHandlerMethod;
        }

        public RequestType RequestType { get; private set; }

        public ResponseType ResponseType { get; private set; }
        
        ////public static RequestHandlerImplementation TryParse(IMethodSymbol method)
        ////{
        ////    // method needs to belong to class
        ////    if (method.ContainingSymbol != null)
        ////    {
        ////        // ResponseType MethodName RequestType
        ////        var returnType = method.ReturnType;
        ////        var parameterType = method.Parameters.FirstOrDefault()?.Type;

        ////        var typeResolver = TypeResolver.Instance;

        ////        var responseType = typeResolver.TryResolveResponseType(returnType);
        ////        var requestType = typeResolver.TryResolveRequestType(parameterType);

        ////        if (responseType != null && requestType != null)
        ////        {
        ////            return new RequestHandlerImplementation(requestType, responseType, method);
        ////        }
        ////    }

        ////    return null;
        ////}

        public static RequestHandlerDefinition TryParse(IMethodSymbol method)
        {
            // method needs to belong to class
            if (method.ContainingSymbol != null)
            {
                string methodDoc = method.GetDocumentationCommentXml();
                var doc = new DocumentationAnalyzer(methodDoc);

                // REQUEST
                var requetMembers = method.Parameters
                    .Where(p => p.RefKind == RefKind.None)
                    .Select(p => CreateMemberFromParameter(p, doc));

                var requestType = new RequestType(method.Name + "Request", requetMembers, doc.Summary);

                // RESPONSE
                var responseMembers = method.Parameters
                    .Where(p => p.RefKind == RefKind.Out)
                    .Select(p => CreateMemberFromParameter(p, doc));

                if (!method.ReturnsVoid)
                {
                    responseMembers = responseMembers.Concat(new[] { CreateMemberFromReturnType(method.ReturnType, doc) });
                }

                string responseDocumentation = $"The response for {{{requestType.Name}}}.";

                ResponseType responseType = responseMembers.Any()
                    ? new ResponseType(method.Name + "Response", responseMembers, responseDocumentation)
                    : ResponseType.VoidResponse;

                return new RequestHandlerDefinition(requestType, responseType, method);
            }

            return null;
        }

        private static Member CreateMemberFromParameter(IParameterSymbol parameter, DocumentationAnalyzer doc)
        {           
            return new Member(parameter.Name, parameter.Type, doc.GetParameter(parameter.Name));
        }

        private static Member CreateMemberFromReturnType(ITypeSymbol type, DocumentationAnalyzer doc)
        {
            return new Member(type.Name, type, doc.Returns);
        }
    }
}
