using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace GleamVault.Converters
{
    public class DiscountPriceColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is float floatValue && floatValue > 0)
            {
                return Colors.Red;
            }
            if (value is double doubleValue && doubleValue > 0)
            {
                return Colors.Red;
            }
            if (value is int intValue && intValue > 0)
            {
                return Colors.Red;
            }
            return Application.Current?.Resources.TryGetValue("Primary", out var primaryColor) == true
                ? primaryColor
                : Color.FromArgb("#00D4B5");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

