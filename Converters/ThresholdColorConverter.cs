
using SkyeMinder.Services;
using System.Globalization;

namespace SkyeMinder.Converters
{
    public class ThresholdColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int number)
                return Colors.Black;

            int low = UserSettings.LowThreshold;
            int high = UserSettings.HighThreshold;

            if (number < low)
                return Colors.Orange;

            if (number > high)
                return Colors.Red;

            return Colors.Black;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => null;
    }
}
