
using Microsoft.Maui.Controls;

namespace SkyeMinder.Pages
{
    public partial class Import : ContentPage
    {
        public Import()
        {
            InitializeComponent();
        }

        private async void OnImportCsvClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select Skye Minder CSV"
            });

            if (result == null)
                return;

            // First dialog: purge or merge
            bool purge = await DisplayAlertAsync(
                "Import CSV",
                "Do you want to delete all current entries before importing?",
                "Delete All First",
                "Keep Existing"
            );

            // If user chose purge
            if (purge)
            {
                bool confirmPurge = await DisplayAlertAsync(
                    "Confirm Delete",
                    "Are you absolutely sure you want to delete ALL current entries? This cannot be undone.",
                    "Yes, Delete All",
                    "Cancel"
                );

                if (!confirmPurge)
                    return; // user tapped outside or canceled
            }
            else
            {
                // User chose merge OR tapped outside
                bool confirmMerge = await DisplayAlertAsync(
                    "Confirm Merge",
                    "Import and merge with existing entries?",
                    "Yes",
                    "Cancel"
                );

                if (!confirmMerge)
                    return; // user tapped outside or canceled
            }

            // Safe to import now
            int count = await App.Database.ImportCsvAsync(result.FullPath, purge);
            
            await DisplayAlertAsync("Import Complete", $"{count} entries imported.", "OK");
        }

    }
}
