using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handyman.Settings;
using Microsoft.CodeAnalysis;

namespace Handyman.Types
{
    /// <summary>
    /// Represents a response type.
    /// </summary>
    public class ResponseType : MemberedBaseType
    {
        public ResponseType(string name, IEnumerable<Member> members, string documentation, string baseClassFQN)
            : base(name, baseClassFQN, members, documentation)
        {
        }

        public bool IsVoidResponse { get; set; } = false;
    }
}
