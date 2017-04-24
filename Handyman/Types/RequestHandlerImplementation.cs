using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Types
{
    /// <summary>
    /// Represents the implementation of a request handler.
    /// </summary>
    public class RequestHandlerImplementation
    {
        private IMethodSymbol requestHandlerMethod;

        public RequestHandlerImplementation(RequestType requestType, ResponseType responseType, IMethodSymbol requestHandlerMethod)
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

        public static RequestHandlerImplementation TryParse(IMethodSymbol method)
        {
            // method needs to belong to class
            if (method.ContainingSymbol != null)
            {
                var responseMembers = method.Parameters
                    .Where(p => p.RefKind == RefKind.Out)
                    .Select(p => new Member(p.Name, p.Type));

                if (!method.ReturnsVoid)
                {
                    responseMembers = responseMembers.Concat(new[] { new Member(method.ReturnType.Name, method.ReturnType) });
                }

                var responseType = new ResponseType(method.Name + "Response", responseMembers);

                var requetMembers = method.Parameters
                    .Where(p => p.RefKind == RefKind.None)
                    .Select(p => new Member(p.Name, p.Type));

                var requestType = new RequestType(method.Name + "Request", requetMembers);

                return new RequestHandlerImplementation(requestType, responseType, method);
            }

            return null;
        }
    }
}
