using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommerceRuntimeHandyman.Types
{
    /// <summary>
    /// Represents the implementation of a request handler.
    /// </summary>
    public class RequestHandlerImplementation
    {
        public RequestHandlerImplementation(RequestType requestType, ResponseType responseType, ClassDeclarationSyntax requestHandlerClassDeclartion)
        {
            this.RequestType = requestType;
            this.ResponseType = responseType;
        }

        public RequestType RequestType { get; private set; }

        public ResponseType ResponseType { get; private set; }

        public static async Task<RequestHandlerImplementation> TryParse(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            // ResponseType MethodName RequestType
            var responseSyntaxType = methodDeclarationSyntax.ReturnType;
            var requestSyntaxType = methodDeclarationSyntax.ParameterList.Parameters.FirstOrDefault()?.Type;

            var typeManager = TypeManager.Instance;

            var responseType = await typeManager.TryResolveResponseType(responseSyntaxType);
            var requestType = await typeManager.TryResolveRequestType(responseSyntaxType);

            if (requestType != null && requestType != null)
            {
                var classDeclaration = methodDeclarationSyntax.FirstAncestorOrSelf<ClassDeclarationSyntax>();
                if (classDeclaration != null)
                {
                    return new RequestHandlerImplementation(requestType, responseType, classDeclaration);
                }
            }

            return null;
        }
    }
}
