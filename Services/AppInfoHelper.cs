
using Microsoft.Maui.ApplicationModel;

namespace SkyeMinder.Services
{
    public static class AppInfoHelper
    {
        public static string AppVersion => $"v{AppInfo.Current.VersionString}";
        public static string FullVersion => $"v{AppInfo.Current.VersionString} (Build {AppInfo.Current.BuildString})";
    }
}