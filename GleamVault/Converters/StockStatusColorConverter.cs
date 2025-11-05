using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Converters
{
    public class StockStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int stock)
            {
                return stock switch
                {
                    0 => Colors.Red,           // Out of stock
                    1 => Colors.Orange,        // Last piece
                    < 5 => Colors.Orange,      // Low stock
                    _ => Colors.Green          // In stock
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
