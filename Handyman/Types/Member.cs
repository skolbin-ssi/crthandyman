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
        public Member(string name, ITypeSymbol type, string documentation = "")
        {
            this.Name = name;
            this.Type = type;
            this.Documentation = documentation;
        }

        public string Name { get; private set; }

        public ITypeSymbol Type { get; private set; }

        public string Documentation { get; private set; }
    }
}
