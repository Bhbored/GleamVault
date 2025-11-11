using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                return floatValue == 0 ? string.Empty : floatValue.ToString();
            }
            if (value is double doubleValue)
            {
                return doubleValue == 0 ? string.Empty : doubleValue.ToString();
            }
            if (value is int intValue)
            {
                return intValue == 0 ? string.Empty : intValue.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return 0f;
                }
                if (float.TryParse(str, out float floatResult))
                {
                    return floatResult;
                }
            }
            return 0f;
        }
    }
}

