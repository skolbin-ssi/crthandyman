using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommerceRuntimeHandyman.Types
{
    public class TypeResolver
    {
        private SemanticModel semanticModel;

        //public TypeResolver(SemanticModel semanticModel)
        //{
        //    this.semanticModel = semanticModel;
        //}

        public static TypeResolver Instance { get { return new TypeResolver(); } }

        /// <summary>
        /// Tries to resolve a request type.
        /// </summary>
        /// <param name="type">The type symbol.</param>
        /// <returns>The request type.</returns>
        public RequestType TryResolveRequestType(ITypeSymbol type)
        {
            if (ImplementsInterface(type, "IRequest"))
            {
                return new RequestType(type);
            }

            return null;
        }

        /// <summary>
        /// Tries to resolve a response type.
        /// </summary>
        /// <param name="type">The type symbol.</param>
        /// <returns>The response type.</returns>
        public ResponseType TryResolveResponseType(ITypeSymbol type)
        {
            if (ImplementsInterface(type, "IResponse"))
            {
                return new ResponseType(type);
            }

            return null;
        }

        private bool ImplementsInterface(ITypeSymbol symbol, string interfaceName)
        {
            return symbol.AllInterfaces.Any(i => i.Name == interfaceName);
        }
    }
}
