using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class QuantityTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int quantity)
            {
                return $"Qty: {quantity}";
            }
            if (value is float floatQty)
            {
                return $"Qty: {floatQty:F0}";
            }
            return "Qty: 0";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

