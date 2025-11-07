using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Converters
{
    internal class HallmarkColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is HallmarkType type)
            {
                return type switch
                {
                    HallmarkType.Gold21K => Colors.Gold,
                    HallmarkType.Gold18K => Colors.Gold,
                    HallmarkType.Gold14K=> Colors.Gold,
                    HallmarkType.Gold10K => Colors.Gold,
                    HallmarkType.Gold9K => Colors.Gold,
                    HallmarkType.Sterling925 => Colors.Silver,
                    HallmarkType.LuxuryBrands => Colors.Purple,
                    _ => Colors.Purple
                };
            }
            return Colors.Purple;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
