
using Android.Content;
using Android.OS;

namespace SkyeMinder
{
    public static partial class ReminderReliability
    {
        [global::System.Runtime.Versioning.SupportedOSPlatform("android23.0")]
        public static partial bool IsBatteryOptimizationIgnored()
        {
            var context = Android.App.Application.Context;
            var service = context.GetSystemService(Context.PowerService);
            if (service is not PowerManager pm)
                return false;

            // True = user has set Battery → Unrestricted
            // False = still Optimized (bad for reminders)
            return pm.IsIgnoringBatteryOptimizations(context.PackageName);
        }
    }
}
