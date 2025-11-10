using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                return floatValue.ToString("#,##0");
            }
            if (value is double doubleValue)
            {
                return doubleValue.ToString("#,##0");
            }
            if (value is int intValue)
            {
                return intValue.ToString("#,##0");
            }
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("#,##0");
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

