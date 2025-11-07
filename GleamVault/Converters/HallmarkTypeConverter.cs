using GleamVault.Utility;
using PropertyChanged;
using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Converters
{
    [AddINotifyPropertyChangedInterface]

    public class HallmarkTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HallmarkType hallmarkType)
            {
                return HallmarkTypeHelper.ToString(hallmarkType);
            }
            return "Luxury Brands";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
