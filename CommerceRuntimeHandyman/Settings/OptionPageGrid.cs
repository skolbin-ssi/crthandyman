using System.ComponentModel;
using Handyman.Settings;
using Microsoft.VisualStudio.Shell;

namespace CommerceRuntimeHandyman.Settings
{
    public class OptionPageGrid : DialogPage, ISettings
    {
        internal const string Category = "CommerceRuntimeHandyman";
        internal const string Name = "Options";

        private const string InterfacesCategoryName = "Interface names";

        private Handyman.Settings.Settings settings = new Handyman.Settings.Settings();

        [Category(InterfacesCategoryName)]
        [DisplayName("Request interface name")]
        [Description("The name of the interface that represents a request message.")]
        public string RequestInterfaceName
        {
            get { return this.settings.RequestInterfaceName; }
            set { this.settings.RequestInterfaceName = value; }
        }

        [Category(InterfacesCategoryName)]
        [DisplayName("Response interface name")]
        [Description("The name of the interface that represents a response message.")]
        public string ResponseInterfaceName
        {
            get { return this.settings.ResponseInterfaceName; }
            set { this.settings.ResponseInterfaceName = value; }
        }

        [Category(InterfacesCategoryName)]
        [DisplayName("Request handler interface name")]
        [Description("The name of the interface that represents a request handler.")]
        public string HandlerInterfaceName
        {
            get { return this.settings.HandlerInterfaceName; }
            set { this.settings.HandlerInterfaceName = value; }
        }

        public void UpdateSettings()
        {
            Handyman.Settings.SettingsManager.UpdateSettings(this.settings);
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                this.UpdateSettings();
            }
        }
    }
}