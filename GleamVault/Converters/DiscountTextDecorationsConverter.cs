using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class DiscountTextDecorationsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is float floatValue && floatValue > 0)
            {
                return TextDecorations.Strikethrough;
            }
            if (value is double doubleValue && doubleValue > 0)
            {
                return TextDecorations.Strikethrough;
            }
            if (value is int intValue && intValue > 0)
            {
                return TextDecorations.Strikethrough;
            }
            return TextDecorations.None;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

