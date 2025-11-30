using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Shared.Models.Enums;

namespace GleamVault.Converters
{
    public class TransactionTypeFirstCharConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TransactionType type)
            {
                var typeString = type switch
                {
                    TransactionType.Sell => "Sell",
                    TransactionType.CustomeOrder => "Custom Order",
                    TransactionType.Repairement => "Repair",
                    TransactionType.Buy => "Buy",
                    _ => value.ToString()
                };
                
                if (!string.IsNullOrEmpty(typeString))
                {
                    return typeString[0].ToString().ToUpper();
                }
            }
            return "?";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

