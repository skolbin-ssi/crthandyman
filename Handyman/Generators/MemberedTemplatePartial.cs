using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handyman.Types;

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
                yield return member.Type.Name + " " + member.Name;
            }
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
