using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price)
            {
                return $"$ {price:F2}";
            }

            if (value is double doublePrice)
            {
                return $"$ {doublePrice:F2}";
            }

            return "$ 0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                str = str.Replace("$", "")
                        .Replace(" ", "")
                        .Trim();

                if (decimal.TryParse(str, NumberStyles.Any, culture ?? CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }
            }
            return 0m;
        }
    }
}
