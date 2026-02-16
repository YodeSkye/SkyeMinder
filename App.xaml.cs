using Plugin.LocalNotification;
using SkyeMinder.Services;
using SkyeMinder.Pages;

namespace SkyeMinder
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; } = null!;
        
        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "bloodsugar.db3");
            Database = new DatabaseService(dbPath);

            Routing.RegisterRoute("MainPage", typeof(MainPage));
            LocalNotificationCenter.Current.NotificationActionTapped += async e =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.GoToAsync("//MainPage");
                });
            };
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}