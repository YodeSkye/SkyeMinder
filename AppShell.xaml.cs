
using SkyeMinder.Pages;

namespace SkyeMinder
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));
            Routing.RegisterRoute(nameof(About), typeof(About));
        }
    }
}