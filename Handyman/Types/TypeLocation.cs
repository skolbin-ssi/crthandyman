using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Handyman.Types
{
    /// <summary>
    /// Represents the location of a type.
    /// </summary>
    public sealed class TypeLocation
    {
        public ITypeSymbol TypeSymbol { get; set; }

        public Location Location { get; set; }

        public SyntaxNode SyntaxNode { get; set; }
    }
}
