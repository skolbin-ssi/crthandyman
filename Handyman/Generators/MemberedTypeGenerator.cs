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

        ////private void CreateClass(MemberedBaseType memberedType)
        ////{
        ////    var ns = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Sample")).NormalizeWhitespace();            

        ////    var clazz = SyntaxFactory.ClassDeclaration(memberedType.Name)
        ////        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));                

        ////    if (!string.IsNullOrWhiteSpace(memberedType.BaseClassName))
        ////    {
        ////        clazz = clazz.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(memberedType.BaseClassName)));
        ////    }            

        ////    ns.AddMembers()
        ////}
    }
}