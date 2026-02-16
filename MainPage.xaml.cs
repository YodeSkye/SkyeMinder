
using Android;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Microsoft.Maui.ApplicationModel;
using SkyeMinder.Models;
using System.Collections.ObjectModel;

namespace SkyeMinder
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Models.BloodSugarEntry> Entries { get; set; } = [];

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void LoadEntries()
        {
            Entries.Clear();
            foreach (var item in await App.Database.GetEntriesAsync(20))
            {
                Entries.Add(item);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            LoadEntries();

            // Focus the entry so the keyboard pops up
            await Task.Delay(300);
            valueEntry.Focus();


#if ANDROID
#pragma warning disable CA1416
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Tiramisu)
            {
                var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
                var permissionCheck = AndroidX.Core.Content.ContextCompat.CheckSelfPermission(activity, Android.Manifest.Permission.PostNotifications);

                if (permissionCheck != Android.Content.PM.Permission.Granted)
                {
                    AndroidX.Core.App.ActivityCompat.RequestPermissions(activity, [Android.Manifest.Permission.PostNotifications], 0);
                }
            }
#pragma warning restore CA1416
#endif
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (int.TryParse(valueEntry.Text, out int value))
            {
                var entry = new Models.BloodSugarEntry
                {
                    Value = value,
                    Notes = notesEditor.Text
                };

                await App.Database.SaveEntryAsync(entry);
                LoadEntries();

                valueEntry.Text = string.Empty;
                notesEditor.Text = string.Empty;
            }
            else
            {
                await DisplayAlertAsync("Error", "Please enter a valid number.", "OK");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is not BloodSugarEntry entry) return;

            string newValue = await DisplayPromptAsync("Edit Value", "Update blood sugar value:", initialValue: entry.Value.ToString());
            if (int.TryParse(newValue, out int updatedValue))
            {
                entry.Value = updatedValue;
                entry.Timestamp = DateTime.Now; // optional: update timestamp
            }

            string newNotes = await DisplayPromptAsync("Edit Notes", "Update notes:", initialValue: entry.Notes);
            if (newNotes != null)
            {
                entry.Notes = newNotes;
            }

            entry.Timestamp = DateTime.Now;
            await App.Database.UpdateEntryAsync(entry);
            LoadEntries();

            logView.ItemsSource = null;
            logView.ItemsSource = Entries;

        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is not BloodSugarEntry entry) return;

            bool confirm = await DisplayAlertAsync("Delete Entry", "Are you sure?", "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteEntryAsync(entry); // 🔥 remove from DB
                Entries.Remove(entry);                      // 🧼 remove from UI
            }
        }
    }
}
