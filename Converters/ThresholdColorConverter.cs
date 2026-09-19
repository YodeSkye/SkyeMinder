
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

            bool isDark = IsDarkMode();

            if (number < low)
                return isDark ? Color.FromArgb("#FFB74D") : Color.FromArgb("#FF8C00");

            if (number > high)
                return isDark ? Color.FromArgb("#EF5350") : Color.FromArgb("#D32F2F");

            return GetDefaultColor();
        }

        private static Color GetDefaultColor()
        {
            return IsDarkMode() ? Colors.White : Colors.Black;
        }

        private static bool IsDarkMode()
        {
            if (Application.Current == null)
                return false;

            var appTheme = Application.Current.UserAppTheme;
            if (appTheme == AppTheme.Unspecified)
            {
                appTheme = Application.Current.RequestedTheme;
            }

            return appTheme == AppTheme.Dark;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => null;
    }
}
