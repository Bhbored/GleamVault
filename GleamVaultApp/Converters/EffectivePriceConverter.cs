using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Shared.Models;

namespace GleamVault.Converters
{
    public class EffectivePriceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Product product)
            {
                // If OfferPrice > 0, use OfferPrice, otherwise use UnitPrice
                float effectivePrice = product.OfferPrice > 0 ? product.OfferPrice : product.UnitPrice;
                return $"$ {effectivePrice:F2}";
            }
            return "$ 0.00";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

