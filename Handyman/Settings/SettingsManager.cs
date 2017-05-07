namespace Handyman.Settings
{
    public static class SettingsManager
    {
        public static ISettings Instance { get; private set; } = new Settings();

        public static void UpdateSettings(ISettings settings)
        {
            Instance = settings;
        }
    }
}
