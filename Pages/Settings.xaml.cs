
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
    }
}
