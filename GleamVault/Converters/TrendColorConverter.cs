using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class TrendColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                return floatValue >= 0 ? Color.FromArgb("#16A34A") : Color.FromArgb("#DC2626");
            }
            if (value is double doubleValue)
            {
                return doubleValue >= 0 ? Color.FromArgb("#16A34A") : Color.FromArgb("#DC2626");
            }
            return Color.FromArgb("#718096");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

