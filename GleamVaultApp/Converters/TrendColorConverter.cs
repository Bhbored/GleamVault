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
                return floatValue >= 0f ? Colors.Green : Colors.Red;
            }
            if (value is double doubleValue)
            {
                return doubleValue >= 0 ? Colors.Green : Colors.Red;
            }
            if (value is decimal decimalValue)
            {
                return decimalValue >= 0 ? Colors.Green : Colors.Red;
            }
            return Colors.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

