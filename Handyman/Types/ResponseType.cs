using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Handyman.Types
{
    /// <summary>
    /// Represents a response type.
    /// </summary>
    public class ResponseType : MemberedBaseType
    {
        public ResponseType(string name, IEnumerable<Member> members)
            : base(name, "IResponse", members)
        {
        }
    }
}
