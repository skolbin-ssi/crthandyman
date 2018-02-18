using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handyman.Types;

namespace Handyman
{
    /// <summary>
    /// Represents information associated to the commerce runtime.
    /// </summary>
    public sealed class CommerceRuntimeReference
    {
        public string RequestBaseClassFqn { get; set; }

        public string ResponseBaseClassFqn { get; set; }

        /// <summary>
        /// Represents a void response (i.e. no response).
        /// </summary>
        public ResponseType VoidResponse { get; set; }
    }
}
