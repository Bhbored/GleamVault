using Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Converters
{
    public class CategoryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Category category)
            {
                return category?.Name?.ToLower() switch
                {
                    "rings" => Color.FromArgb("#FF6B8E"),       // Coral pink
                    "necklaces" => Color.FromArgb("#4ECDC4"),   // Teal
                    "earrings" => Color.FromArgb("#45B7D1"),    // Sky blue
                    "bracelets" => Color.FromArgb("#96CEB4"),   // Mint green
                    "watches" => Color.FromArgb("#0389ff"),     // Light yellow
                    "brooches" => Color.FromArgb("#DDA0DD"),    // Plum
                    "cufflinks" => Color.FromArgb("#87CEEB"),   // Light sky blue
                    "anklets" => Color.FromArgb("#98FB98"),     // Pale green
                    "charms" => Color.FromArgb("#FFD700"),      // Gold
                    "bridal" => Color.FromArgb("#FFB6C1"),      // Light pink
                    _ => Colors.Gray
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
