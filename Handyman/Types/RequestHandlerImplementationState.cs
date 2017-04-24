using System;

namespace Handyman.Types
{
    /// <summary>
    /// Represents the state in which a request handler implementation is in.
    /// </summary>
    [Flags]
    public enum RequestHandlerImplementationState
    {
        /// <summary>
        /// The handler is implemeneted and point to valid request and response types.
        /// </summary>
        Complete = 0,

        /// <summary>
        /// The handler does not have a valid request type.
        /// </summary>
        MissingRequest = 1,

        /// <summary>
        /// The handler does not have a valid response type.
        /// </summary>
        MissingResponse = 2,
    }
}
