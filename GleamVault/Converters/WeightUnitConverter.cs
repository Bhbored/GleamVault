using GleamVault.Utility;
using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Converters
{
    public class WeightUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WeightUnit weightUnit)
            {
                return WeightUnitHelper.ToString(weightUnit); 
            }
            return "Grams";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
