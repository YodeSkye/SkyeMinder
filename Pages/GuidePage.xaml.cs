
namespace SkyeMinder.Pages
{
    public partial class GuidePage : ContentPage
    {
        public GuidePage()
        {
            InitializeComponent();
        }
        //private void OnWhyExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
        //{
        //    WhyChevron.Rotation = e.IsExpanded ? 0 : 90;
        //}
        //private void OnAutoRevokeExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
        //{
        //    AutoRevokeChevron.Rotation = e.IsExpanded ? 0 : 90;
        //}
        [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
        private void OnOpenSettingsClicked(object sender, EventArgs e)
        {
#if ANDROID
            var context = Android.App.Application.Context;
            var intent = new Android.Content.Intent(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
            intent.SetFlags(Android.Content.ActivityFlags.NewTask);
            context.StartActivity(intent);
#endif
        }
    }
}
