using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Converters
{
    public class StockStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int stock)
            {
                return stock switch
                {
                    0 => "OUT OF STOCK",
                    1 => "LAST PIECE",
                    < 5 => "LOW STOCK",
                    _ => $"{stock} IN STOCK"
                };
            }
            return "CHECK STOCK";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
