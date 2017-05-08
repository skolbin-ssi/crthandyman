namespace Handyman.Settings
{
    public static class SettingsManager
    {
        public static IGlobalSettings Instance { get; private set; } = new GlobalSettings();

        public static void UpdateSettings(IGlobalSettings settings)
        {
            Instance = settings;
        }
    }
}
