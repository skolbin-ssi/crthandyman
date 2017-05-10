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

        private string ConstructorArguments
        {
            get
            {
                return string.Join(", ", this.GetTypedMember());
            }
        }

        private IEnumerable<string> GetTypedMember()
        {
            foreach (var member in this.m.Members)
            {
                yield return GetToken(member.Type) + " " + ToFirstCharLower(member.Name);
            }
        }

        private static string GetToken(ISymbol symbol)
        {
            return symbol.ToDisplayString();
        }

        private static string ToFirstCharUpper(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var chars = value.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }

        private static string ToFirstCharLower(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var chars = value.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
    }
}
