
using Android.App;
using Android.Content;

namespace SkyeMinder
{
    public static partial class ReminderReliability
    {
        public static partial bool IsBatteryOptimizationIgnored();

        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        public static bool NeedsAttention()
        {
            var batteryOk = IsBatteryOptimizationIgnored();

            return !batteryOk;
        }
        public static class BatteryOptimizationHelper
        {
            [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
            public static Task<bool> IsIgnoringBatteryOptimizationsAsync()
            {
#if ANDROID
                return Task.FromResult(ReminderReliability.IsBatteryOptimizationIgnored());
#else
        return Task.FromResult(true);
#endif
            }

            [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
            public static Task OpenBatteryOptimizationSettingsAsync()
            {
#if ANDROID
                ReminderReliability.OpenBatteryOptimizationSettings();
#endif
                return Task.CompletedTask;
            }
        }
        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        public static void OpenBatteryOptimizationSettings()
        {
            var context = Android.App.Application.Context;
            var intent = new Intent(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
            intent.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

    }

}
