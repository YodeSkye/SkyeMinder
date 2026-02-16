using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Java.Util.Concurrent;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
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

    private async void AddReminder_Clicked(object sender, EventArgs e)
    {
        var selectedTime = timePicker.Time ?? TimeSpan.Zero;

        var reminder = new ReminderTime { TimeOfDay = selectedTime };
        await App.Database.AddReminderTimeAsync(reminder);
        await LoadReminders();

        //await ScheduleReminderNotification(reminder);
        ReminderScheduler.ScheduleAll();
    }
    
    private async void DeleteReminder_Clicked(object sender, EventArgs e)
    {
        var id = (int)((Button)sender).CommandParameter;

        // Cancel the scheduled notification
        LocalNotificationCenter.Current.Cancel(id);

        await App.Database.DeleteReminderTimeAsync(id);
        await LoadReminders();
    }
   
    //private async Task ScheduleReminderNotification(ReminderTime reminder)
    //{
    //    var now = DateTime.Now;
    //    var triggerTime = new DateTime(
    //        now.Year,
    //        now.Month,
    //        now.Day,
    //        reminder.TimeOfDay.Hours,
    //        reminder.TimeOfDay.Minutes,
    //        0
    //    );


    //    // If the time is already past for today, schedule it for tomorrow
    //    if (triggerTime < now)
    //        triggerTime = triggerTime.AddDays(1);

    //    await LocalNotificationCenter.Current.Show(new NotificationRequest
    //    {
    //        NotificationId = reminder.Id * 1000, // Unique ID for one-time
    //        Title = "Skye Minder Notification",
    //        Description = $"It's {triggerTime:hh\\:mm tt}, time to log your blood sugar!",
    //        Android = new AndroidOptions
    //        {
    //            ChannelId = "skyeminder_reminders_channel",
    //            Priority = AndroidPriority.High,
    //        },
    //        Schedule = new NotificationRequestSchedule
    //        {
    //            NotifyTime = triggerTime,
    //            RepeatType = NotificationRepeat.No
    //        }
    //    });

    //    var tomorrow = triggerTime.AddDays(1);

    //    await LocalNotificationCenter.Current.Show(new NotificationRequest
    //    {
    //        NotificationId = reminder.Id, // Original ID for daily repeat
    //        Title = "Skye Minder Notification",
    //        Description = $"It's {triggerTime:hh\\:mm tt}, time to log your blood sugar!",
    //        Android = new AndroidOptions
    //        {
    //            ChannelId = "skyeminder_reminders_channel",
    //            Priority = AndroidPriority.High,
    //        },
    //        Schedule = new NotificationRequestSchedule
    //        {
    //            NotifyTime = tomorrow,
    //            RepeatType = NotificationRepeat.Daily
    //        }
    //    });
    //}
}