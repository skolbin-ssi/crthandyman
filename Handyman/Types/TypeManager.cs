using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommerceRuntimeHandyman.Types
{
    public class TypeManager
    {
        public static TypeManager Instance
        {
            get { return new TypeManager(); }
        }

        /// <summary>
        /// Tries to resolve a request type.
        /// </summary>
        /// <param name="type">The type syntax.</param>
        /// <returns>A task.</returns>
        public Task<RequestType> TryResolveRequestType(TypeSyntax type)
        {
            return null;
        }

        /// <summary>
        /// Tries to resolve a response type.
        /// </summary>
        /// <param name="type">The type syntax.</param>
        /// <returns>A task.</returns>
        public Task<ResponseType> TryResolveResponseType(TypeSyntax type)
        {
            return null;
        }
    }
}
