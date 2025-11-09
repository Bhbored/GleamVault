using Microsoft.Maui.Controls;
using Shared.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace GleamVault.Converters;

public class IsProductNotSelectedConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ObservableCollection<Product> selectedProducts && parameter is Product product)
        {
            return !selectedProducts.Contains(product);
        }
        return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

