using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CommerceRuntimeHandyman.Types
{
    /// <summary>
    /// Represents a response type.
    /// </summary>
    public class ResponseType
    {
        public ResponseType(ITypeSymbol symbol)
        {
            this.Symbol = symbol;
        }

        public ITypeSymbol Symbol { get; private set; }

        public string Name { get { return this.Symbol.Name; } }
    }
}
