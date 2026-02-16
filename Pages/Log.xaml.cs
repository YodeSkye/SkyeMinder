
using SkyeMinder.Models;
using System.Collections.ObjectModel;

namespace SkyeMinder.Pages
{
    public partial class Log : ContentPage
    {
        public ObservableCollection<BloodSugarEntry> entries { get; set; } = new();
	    
        public Log()
	    {
		    InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadEntries(); // ?? refresh every time the page appears
        }

        private async void LoadEntries()
        {
            var freshEntries = await App.Database.GetAllEntriesAsync(); // or more if you want full history
            entries.Clear();
            foreach (var entry in freshEntries)
            {
                entries.Add(entry);
            }
        }
    }
}