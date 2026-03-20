
using SkyeMinder.Services;

namespace SkyeMinder.Pages
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();

            LowEntry.Text = UserSettings.LowThreshold.ToString();
            HighEntry.Text = UserSettings.HighThreshold.ToString();
        }

        private void OnLowChanged(object sender, TextChangedEventArgs e)
        {
            // Blank means "no value yet" — do not save
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                return;

            if (int.TryParse(e.NewTextValue, out int low))
                UserSettings.LowThreshold = low;
        }

        private void OnHighChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                return;

            if (int.TryParse(e.NewTextValue, out int high))
                UserSettings.HighThreshold = high;
        }
        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        private async void OnDebugReminderStatusClicked(object sender, EventArgs e)
        {
            // Call your shared logic
            var batteryOk = ReminderReliability.IsBatteryOptimizationIgnored();
            var autoRevokeEnabled = ReminderReliability.IsAutoRevokeEnabled();

            // Build a clean message
            var msg =
                $"Battery Optimization Ignored: {batteryOk}\n" +
                $"Auto‑Revoke Enabled: {autoRevokeEnabled}";

            await DisplayAlertAsync("Reminder Debug", msg, "OK");
        }
    }
}
