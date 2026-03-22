
using Plugin.LocalNotification;
using SkyeMinder.Models;
using SkyeMinder.Services;

namespace SkyeMinder.Pages;

public partial class ReminderSettings : ContentPage
{
    public ReminderSettings()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadReminders();

    }

    private async Task LoadReminders()
    {
        var reminders = await App.Database.GetReminderTimesAsync();
        reminderListView.ItemsSource = reminders;
    }

    [global::System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
    private async void AddReminder_Clicked(object sender, EventArgs e)
    {
        var selectedTime = timePicker.Time ?? TimeSpan.Zero;

        var reminder = new ReminderTime { TimeOfDay = selectedTime };
        await App.Database.AddReminderTimeAsync(reminder);
        await LoadReminders();

        ReminderScheduler.ScheduleAll();

        var reminders = await App.Database.GetReminderTimesAsync();
        if (reminders.Count == 1) // first reminder ever
        {
            bool needsAttention = ReminderReliability.NeedsAttention();

            if (needsAttention)
            {
                await Navigation.PushAsync(new GuidePage());
            }
        }
    }

    private async void DeleteReminder_Clicked(object sender, EventArgs e)
    {
        var id = (int)((Button)sender).CommandParameter;

        // Cancel the scheduled notification
        LocalNotificationCenter.Current.Cancel(id);

        await App.Database.DeleteReminderTimeAsync(id);
        await LoadReminders();
    }
}