
namespace SkyeMinder.Pages
{
    public partial class GuidePage : ContentPage
    {
        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        public GuidePage()
        {
            InitializeComponent();
            UpdateBatteryStatus();
        }

        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        private async void UpdateBatteryStatus()
        {
            bool isIgnored = await ReminderReliability.BatteryOptimizationHelper.IsIgnoringBatteryOptimizationsAsync();

            BatteryStatusLabel.Text = isIgnored
                ? "🟢 All set — battery optimization is off"
                : "🟡 Still on — tap the button above to fix it";
        }

        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        private async void OnOpenBatteryOptimizationClicked(object sender, EventArgs e)
        {
            await ReminderReliability.BatteryOptimizationHelper.OpenBatteryOptimizationSettingsAsync();
        }
        private void OnWhyExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
        {
            WhyChevron.Rotation = e.IsExpanded ? 0 : 90;
        }
        private void OnAutoRevokeExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
        {
            AutoRevokeChevron.Rotation = e.IsExpanded ? 0 : 90;
        }
    }
}
