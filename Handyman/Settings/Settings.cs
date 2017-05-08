using System;

namespace Handyman.Settings
{
    public class Settings : ISettings
    {
        public string RequestInterfaceFQN { get; set; } = "Microsoft.Dynamics.Commerce.Runtime.IRequest";

        public string ResponseInterfaceFQN { get; set; } = "Microsoft.Dynamics.Commerce.Runtime.IResponse";

        public string HandlerInterfaceFQN { get; set; } = "Microsoft.Dynamics.Commerce.Runtime.IRequestHandler";
    }
}
