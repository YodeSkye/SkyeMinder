
using Microsoft.Maui.Storage;

namespace SkyeMinder.Services
{
    public static class UserSettings
    {
        public static int LowThreshold
        {
            get => Preferences.Get(nameof(LowThreshold), 80);
            set => Preferences.Set(nameof(LowThreshold), value);
        }

        public static int HighThreshold
        {
            get => Preferences.Get(nameof(HighThreshold), 150);
            set => Preferences.Set(nameof(HighThreshold), value);
        }
    }
}
