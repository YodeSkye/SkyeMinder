
using Android.App;
using Android.Util;
using Android.Content;
using Android.Widget;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using SkyeMinder.Models;

namespace SkyeMinder.Services
{
    public static class ReminderScheduler
    {
        public static void ScheduleAll()
        {
            Task.Run(async () =>
            {
                var reminders = await App.Database.GetReminderTimesAsync();

                foreach (var reminder in reminders)
                {
                    LocalNotificationCenter.Current.Cancel(reminder.Id * 1000);
                    LocalNotificationCenter.Current.Cancel(reminder.Id);

                    var now = DateTime.Now;
                    var triggerTime = new DateTime(
                        now.Year,
                        now.Month,
                        now.Day,
                        reminder.TimeOfDay.Hours,
                        reminder.TimeOfDay.Minutes,
                        0);

                    if (triggerTime < now)
                        triggerTime = triggerTime.AddDays(1);
                    // One-time notification for today
                    await LocalNotificationCenter.Current.Show(new NotificationRequest
                    {
                        NotificationId = reminder.Id * 1000, // Unique ID for one-time
                        Title = "Skye Minder Notification",
                        Description = $"It's {triggerTime:hh\\:mm tt}, time to log your blood sugar!",
                        Android = new AndroidOptions
                        {
                            ChannelId = "skyeminder_reminders_channel",
                            Priority = AndroidPriority.High,
                        },
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = triggerTime,
                            RepeatType = NotificationRepeat.No
                        }
                    });

                    // Daily repeating notification starting tomorrow
                    var tomorrow = triggerTime.AddDays(1);

                    await LocalNotificationCenter.Current.Show(new NotificationRequest
                    {
                        NotificationId = reminder.Id, // Original ID for daily repeat
                        Title = "Skye Minder Notification",
                        Description = $"It's {tomorrow:hh\\:mm tt}, time to log your blood sugar!",
                        Android = new AndroidOptions
                        {
                            ChannelId = "skyeminder_reminders_channel",
                            Priority = AndroidPriority.High,
                        },
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = tomorrow,
                            RepeatType = NotificationRepeat.Daily
                        }
                    });
                    //Services.ToastHelper.ShowToast($"Reminder {reminder.Id} Scheduled");
                }
                Services.ToastHelper.ShowToast("Reminders Scheduled");
            });
        }
    }
}