using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Handyman.Types
{
    public class Member
    {
        public Member(string name, ITypeSymbol type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; private set; }

        public ITypeSymbol Type { get; private set; }
    }
}
