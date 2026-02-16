
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SkyeMinder.Models;
using SkyeMinder.Pages;
using SkyeMinder.Services;

namespace SkyeMinder.Pages
{
    public partial class Export : ContentPage
    {
        public Export()
        {
            InitializeComponent();
            DateRangePicker.SelectedIndexChanged += OnDateRangeChanged;
            StartDatePicker.DateSelected += async (s, e) => await UpdateEntryCountAsync();
            EndDatePicker.DateSelected += async (s, e) => await UpdateEntryCountAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DateRangePanel.IsVisible = false;
        }

        private async void OnDateRangeChanged(object? sender, EventArgs e)
        {
            DateRangePanel.IsVisible = DateRangePicker.SelectedIndex == 6;
            await UpdateEntryCountAsync();
        }
        private async void OnExportClicked(object? sender, EventArgs e)
        {
            var format = FormatPicker.SelectedItem?.ToString();
            var includeSummary = IncludeSummaryCheckBox.IsChecked;
            var selectedIndex = DateRangePicker.SelectedIndex;

            List<BloodSugarEntry> entries = selectedIndex switch
            {
                0 => await App.Database.GetAllEntriesAsync(),
                1 => await App.Database.GetEntriesForLastWeekAsync(),
                2 => await App.Database.GetEntriesForLastMonthAsync(),
                3 => await App.Database.GetEntriesForLast3MonthsAsync(),
                4 => await App.Database.GetEntriesForLast6MonthsAsync(),
                5 => await App.Database.GetEntriesForLastYearAsync(),
                6 => await App.Database.GetEntriesInRangeAsync((StartDatePicker.Date ?? DateTime.Today), (EndDatePicker.Date ?? DateTime.Today).AddDays(1).AddTicks(-1)),
                _ => []
            };

            if (entries.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("No Data", "No entries found for the selected range.", "OK");
                return;
            }

            if (format == "CSV")
            {
                var csv = Exporter.GenerateCSV(entries, includeSummary);
                await FileSaver.SaveToDownloadsAsync(csv, "BloodSugarLog.csv");
                await FileSaver.SaveToAppDataAsync(csv, "BloodSugarLog.csv");
                await Shell.Current.DisplayAlertAsync("Export Complete", "CSV saved to Downloads", "OK");
                ShareButton.IsEnabled = true;
            }
            else if (format == "PDF")
            {
                var pdfBytes = Exporter.GeneratePDF(entries, includeSummary);
                var base64 = Convert.ToBase64String(pdfBytes);
                await FileSaver.SaveToDownloadsAsync(base64, "BloodSugarLog.pdf");
                await FileSaver.SaveToAppDataAsync(pdfBytes, "BloodSugarLog.pdf");
                await Shell.Current.DisplayAlertAsync("Export Complete", "PDF saved to Downloads", "OK");
                ShareButton.IsEnabled = true;
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Export Failed", "Please select a valid format.", "OK");
            }
        }
        private async void OnShareClicked(object sender, EventArgs e)
        {
            var format = FormatPicker.SelectedItem?.ToString();
            var fileName = format == "CSV" ? "BloodSugarLog.csv" : "BloodSugarLog.pdf";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(filePath))
            {
                await Shell.Current.DisplayAlertAsync("Share Failed", "Please export the file first.", "OK");
                return;
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Blood Sugar Log",
                File = new ShareFile(filePath)
            });
        }

        private async Task UpdateEntryCountAsync()
        {
            var selectedIndex = DateRangePicker.SelectedIndex;
            List<BloodSugarEntry> entries = selectedIndex switch
            {
                0 => await App.Database.GetAllEntriesAsync(),
                1 => await App.Database.GetEntriesForLastWeekAsync(),
                2 => await App.Database.GetEntriesForLastMonthAsync(),
                3 => await App.Database.GetEntriesForLast3MonthsAsync(),
                4 => await App.Database.GetEntriesForLast6MonthsAsync(),
                5 => await App.Database.GetEntriesForLastYearAsync(),
                6 => await App.Database.GetEntriesInRangeAsync((StartDatePicker.Date ?? DateTime.Today), (EndDatePicker.Date ?? DateTime.Today).AddDays(1).AddTicks(-1)),
                _ => []
            };

            EntryCountLabel.Text = $"Entries: {entries.Count}";
        }
    }
}