using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class TimeframeSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selectedTimeframe && parameter is string timeframe)
            {
                return selectedTimeframe == timeframe ? Color.FromArgb("#00D4B5") : Color.FromArgb("#333333");
            }
            return Color.FromArgb("#333333");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

