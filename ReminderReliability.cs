
namespace SkyeMinder
{
    public static partial class ReminderReliability
    {
        public static partial bool IsBatteryOptimizationIgnored();
        public static partial bool IsAutoRevokeEnabled();

        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        public static bool NeedsAttention()
        {
            var batteryOk = IsBatteryOptimizationIgnored();
            var autoRevokeOk = !IsAutoRevokeEnabled();

            return !(batteryOk && autoRevokeOk);
        }
    }

}
