
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SkyeMinder.Models
{
    public enum ChartTimeRange
    {
        Week,
        Month,
        ThreeMonths,
        SixMonths,
        All
    }

    public class GlucoseChartViewModel : BindableObject
    {
        public ICommand ChangeRangeCommand { get; }

        public GlucoseChartViewModel()
        {
            // Initialize command to handle button clicks or segmented control selections
            ChangeRangeCommand = new Command<ChartTimeRange>(SetTimeRange);
            ChangeRangeCommandEx = new Command<string>(SetTimeRangeFromString);
        }

        // Secondary command to support passing strings from XAML parameters if needed
        public ICommand ChangeRangeCommandEx { get; }

        private ChartTimeRange _selectedRange = ChartTimeRange.Month;
        public ChartTimeRange SelectedRange
        {
            get => _selectedRange;
            set
            {
                if (_selectedRange != value)
                {
                    _selectedRange = value;
                    OnPropertyChanged();
                    _ = LoadEntriesAsync(); // Refresh chart on range change
                }
            }
        }

        private ObservableCollection<BloodSugarEntry> _bloodSugarEntries = [];
        public ObservableCollection<BloodSugarEntry> BloodSugarEntries
        {
            get => _bloodSugarEntries;
            set
            {
                _bloodSugarEntries = value;
                OnPropertyChanged();

                // Notify UI that dependent stats need to redraw
                NotifyStatProperties();
                RecalculateChartBounds();
            }
        }

        public double DynamicMinY { get; set; } = 40;
        public double DynamicMaxY { get; set; } = 300;

        public double AverageValue => BloodSugarEntries.Any() ? Math.Round(BloodSugarEntries.Average(x => x.Value), 1) : 0;
        public int HighestValue => BloodSugarEntries.Any() ? BloodSugarEntries.Max(x => x.Value) : 0;
        public int LowestValue => BloodSugarEntries.Any() ? BloodSugarEntries.Min(x => x.Value) : 0;

        private void SetTimeRange(ChartTimeRange range)
        {
            SelectedRange = range;
        }

        private void SetTimeRangeFromString(string rangeString)
        {
            if (Enum.TryParse<ChartTimeRange>(rangeString, true, out var range))
            {
                SelectedRange = range;
            }
        }

        private void RecalculateChartBounds()
        {
            if (BloodSugarEntries == null || !BloodSugarEntries.Any())
            {
                DynamicMinY = 40;
                DynamicMaxY = 300;
            }
            else
            {
                double minVal = BloodSugarEntries.Min(x => x.Value);
                double maxVal = BloodSugarEntries.Max(x => x.Value);

                // Add 10 mg/dL padding above and below data points
                DynamicMinY = Math.Max(0, Math.Floor((minVal - 10) / 10.0) * 10);
                DynamicMaxY = Math.Ceiling((maxVal + 10) / 10.0) * 10;
            }

            OnPropertyChanged(nameof(DynamicMinY));
            OnPropertyChanged(nameof(DynamicMaxY));
        }

        private void NotifyStatProperties()
        {
            OnPropertyChanged(nameof(AverageValue));
            OnPropertyChanged(nameof(HighestValue));
            OnPropertyChanged(nameof(LowestValue));
        }

        public async Task LoadEntriesAsync()
        {
            var data = SelectedRange switch
            {
                ChartTimeRange.Week => await App.Database.GetEntriesForLastWeekAsync(),
                ChartTimeRange.Month => await App.Database.GetEntriesForLastMonthAsync(),
                ChartTimeRange.ThreeMonths => await App.Database.GetEntriesForLast3MonthsAsync(),
                ChartTimeRange.SixMonths => await App.Database.GetEntriesForLast6MonthsAsync(),
                ChartTimeRange.All => await App.Database.GetAllEntriesAsync(),
                _ => await App.Database.GetEntriesForLastMonthAsync()
            };

            // Order chronologically for the chart line
            var orderedData = data.OrderBy(e => e.Timestamp);

            BloodSugarEntries = new ObservableCollection<BloodSugarEntry>(orderedData);
        }
    }
}
