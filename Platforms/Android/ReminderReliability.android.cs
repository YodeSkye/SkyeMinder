
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;

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

        private const int AUTO_REVOKE_PERMISSIONS_FLAG = 0x00002000;

        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        public static partial bool IsAutoRevokeEnabled()
        {
            var context = Android.App.Application.Context;
            if (context == null)
                return true;

            var pm = context.PackageManager;
            if (pm == null)
                return true;

            var packageName = context.PackageName;
            if (string.IsNullOrEmpty(packageName))
                return true;

            var pkgInfo = pm.GetPackageInfo(packageName, 0);
            if (pkgInfo == null || pkgInfo.ApplicationInfo == null)
                return true;

            return (((int)pkgInfo.ApplicationInfo.Flags) & AUTO_REVOKE_PERMISSIONS_FLAG) != 0;
        }
    }
}
