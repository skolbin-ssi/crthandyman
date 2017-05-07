using System;

namespace Handyman.Settings
{
    public class Settings : ISettings
    {
        public string RequestInterfaceName { get; set; } = "IRequest";

        public string ResponseInterfaceName { get; set; } = "IResponse";

        public string HandlerInterfaceName { get; set; } = "IRequestHandler";
    }
}
