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
        public RequestHandlerImplementation(RequestType requestType, ResponseType responseType, ISymbol requestHandlerClass)
        {
            this.RequestType = requestType;
            this.ResponseType = responseType;
        }

        public RequestType RequestType { get; private set; }

        public ResponseType ResponseType { get; private set; }

        public static RequestHandlerImplementation TryParse(IMethodSymbol method)
        {
            if (method.ContainingSymbol != null)
            {
                // ResponseType MethodName RequestType
                var returnType = method.ReturnType;
                var parameterType = method.Parameters.FirstOrDefault()?.Type;

                var typeResolver = TypeResolver.Instance;

                var responseType = typeResolver.TryResolveResponseType(returnType);
                var requestType = typeResolver.TryResolveRequestType(parameterType);

                if (responseType != null && requestType != null)
                {
                    return new RequestHandlerImplementation(requestType, responseType, method.ContainingSymbol);
                }
            }

            return null;
        }
    }
}
