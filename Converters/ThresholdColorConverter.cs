
using SkyeMinder.Services;
using System.Globalization;

namespace SkyeMinder.Converters
{
    public class ThresholdColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int number)
                return GetDefaultColor();

            int low = UserSettings.LowThreshold;
            int high = UserSettings.HighThreshold;

            if (number < low)
                return Colors.Orange;

            if (number > high)
                return Colors.Red;

            return GetDefaultColor();
        }

        private static Color GetDefaultColor()
        {
            // Check current theme and fallback to high-contrast default
            var currentTheme = Application.Current?.RequestedTheme;
            return currentTheme == AppTheme.Dark ? Colors.White : Colors.Black;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => null;
    }
}
