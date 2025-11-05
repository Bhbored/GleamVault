using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GleamVault.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float price)
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
           throw new NotImplementedException();
        }
    }
}
