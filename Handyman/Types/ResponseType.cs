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
        /// <summary>
        /// Represents a void response (i.e. no response).
        /// </summary>
        public static readonly ResponseType VoidResponse = new ResponseType("NullResponse", new Member[0]);

        public ResponseType(string name, IEnumerable<Member> members)
            : base(name, "IResponse", members)
        {
        }

        public bool IsVoidResponse
        {
            get
            {
                return this == VoidResponse;
            }
        }
    }
}
