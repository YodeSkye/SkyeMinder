
using System.Windows.Input;

namespace SkyeMinder.Pages;

public partial class About : ContentPage
{
#pragma warning disable CA1822
    public string AppVersion => AppInfo.VersionString;
    public string BuildNumber => AppInfo.BuildString;
    public string BuildDate
    {
        get
        {
            try
            {
                var path = AppContext.BaseDirectory;

                if (Directory.Exists(path))
                {
                    var info = new DirectoryInfo(path);
                    return info.LastWriteTime.ToString("yyyy-MM-dd");
                }
            }
            catch { }

            return "Unknown";
        }
    }
    public ICommand OpenHomepageCommand { get; }
    public ICommand OpenGitHubDonateCommand { get; }
    public ICommand OpenPayPalDonateCommand { get; }
#pragma warning restore CA1822

    public About()
    {
        InitializeComponent();

        OpenHomepageCommand = new Command(async () =>
            await Launcher.OpenAsync("https://github.com/YodeSkye"));
        OpenGitHubDonateCommand = new Command(async () =>
            await Launcher.OpenAsync("https://github.com/sponsors/YodeSkye"));
        OpenPayPalDonateCommand = new Command(async () =>
            await Launcher.OpenAsync("https://www.paypal.com/donate/?hosted_button_id=RVH5T9H69G6CS"));

        BindingContext = this;
    }
}