using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Shared.Models.Enums;

namespace GleamVault.Converters
{
    public class ChannelConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SaleChannel channel)
            {
                return $"Channel: {channel}";
            }
            return "Channel: N/A";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

