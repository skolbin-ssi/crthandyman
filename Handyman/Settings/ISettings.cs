namespace Handyman.Settings
{
    /// <summary>
    /// Define settings schema for the Handyman library.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets the full qualified name of the interface for a request.
        /// </summary>
        string RequestInterfaceFQN { get; }

        /// <summary>
        /// Gets the full qualified name of the interface for a response.
        /// </summary>
        string ResponseInterfaceFQN { get; }

        /// <summary>
        /// Gets the full qualified name of the interface for a request handler.
        /// </summary>
        string HandlerInterfaceFQN { get; }
    }
}
