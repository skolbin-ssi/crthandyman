namespace Handyman.Settings
{
    /// <summary>
    /// Define settings schema for the Handyman library.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets the name of the interface for a request.
        /// </summary>
        string RequestInterfaceName { get; }

        /// <summary>
        /// Gets the name of the interface for a response.
        /// </summary>
        string ResponseInterfaceName { get; }

        /// <summary>
        /// Gets the name of the interface for a request handler.
        /// </summary>
        string HandlerInterfaceName { get; }
    }
}
