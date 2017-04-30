using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handyman.Types;
using Microsoft.CodeAnalysis;

namespace Handyman.Generators
{
    public partial class MemberedTemplate : MemberedTemplateBase
    {
        private MemberedBaseType m;

        public MemberedTemplate(MemberedBaseType memberedType)
        {
            this.m = memberedType;
        }

        private IEnumerable<string> GetTypedMember()
        {
            foreach (var member in this.m.Members)
            {
                yield return GetToken(member.Type) + " " + member.Name;
            }
        }

        private static string GetToken(ISymbol symbol)
        {
            return symbol.ToDisplayString();
        }

        private string ToCamelCase(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var chars = str.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
    }
}
