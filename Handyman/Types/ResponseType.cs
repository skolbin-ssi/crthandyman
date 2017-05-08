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
        /// <summary>
        /// Represents a void response (i.e. no response).
        /// </summary>
        public static readonly ResponseType VoidResponse = new ResponseType("NullResponse", new Member[0], string.Empty);

        public ResponseType(string name, IEnumerable<Member> members, string documentation)
            : base(name, SettingsManager.Instance.RequestInterfaceFQN, members, documentation)
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
