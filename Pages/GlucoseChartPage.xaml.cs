
using SkyeMinder.Models;

namespace SkyeMinder.Pages;

public partial class GlucoseChartPage : ContentPage
{
    private readonly GlucoseChartViewModel _viewModel;

    public GlucoseChartPage()
    {
        InitializeComponent();
        _viewModel = new GlucoseChartViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEntriesAsync();
    }
}
