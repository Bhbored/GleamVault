using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Shared.Models.Enums;

namespace GleamVault.Converters
{
    public class TransactionTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TransactionType type)
            {
                return type switch
                {
                    TransactionType.Sell => "Sell",
                    TransactionType.CustomeOrder => "Custom Order",
                    TransactionType.Repairement => "Repair",
                    TransactionType.Buy => "Buy",
                    _ => type.ToString()
                };
            }
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

