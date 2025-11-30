using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Shared.Models.Enums;

namespace GleamVault.Converters
{
    public class TransactionTypeColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TransactionType type)
            {
                return type switch
                {
                    TransactionType.Sell => Color.FromArgb("#4CAF50"),
                    TransactionType.CustomeOrder => Color.FromArgb("#2196F3"),
                    TransactionType.Repairement => Color.FromArgb("#FF9800"),
                    TransactionType.Buy => Color.FromArgb("#9C27B0"),
                    _ => Color.FromArgb("#757575")
                };
            }
            return Color.FromArgb("#757575");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

