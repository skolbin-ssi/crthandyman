using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CommerceRuntimeHandyman.Types
{
    /// <summary>
    /// Represents the a request type.
    /// </summary>
    public class RequestType
    {
        public RequestType(ITypeSymbol symbol)
        {
            this.Symbol = symbol;
        }

        public ITypeSymbol Symbol { get; private set; }

        public string Name { get { return this.Symbol.Name; } }
    }
}