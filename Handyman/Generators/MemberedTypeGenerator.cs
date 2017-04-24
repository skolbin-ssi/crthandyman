using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Handyman.Generators
{
    public class MemberedTypeGenerator
    {
        public string GenerateSyntax(MemberedBaseType memberedType)
        {
            return new MemberedTemplate(memberedType).TransformText();
        }
    }
}