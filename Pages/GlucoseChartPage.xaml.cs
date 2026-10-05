
using SkyeMinder.Models;

namespace SkyeMinder.Pages;

public partial class GlucoseChartPage : ContentPage
{
    private GlucoseChartViewModel? ViewModel => BindingContext as GlucoseChartViewModel;

    public GlucoseChartPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (ViewModel != null)
        {
            // Update low/high threshold line colors or values if settings changed
            ViewModel.RefreshThresholds();

            // Load database entries asynchronously for the selected range
            _ = ViewModel.LoadEntriesAsync();
        }
    }
}
