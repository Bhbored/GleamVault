using GleamVault.MVVM.ViewModels;
using Microsoft.Maui.Controls;
using PropertyChanged;
using Shared.Models;

namespace GleamVault.Converters
{
    [AddINotifyPropertyChangedInterface]

    public class ProductTemplateSelector : DataTemplateSelector
    {
        public ProductVM? ViewModel { get; set; }
       
        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is not Product)
                return null;

            var key = (ViewModel?.IsDataLoading == true ) ? "ShimmerTemplate" : "ProductTemplate";

            Application.Current!.Resources.TryGetValue(key, out var template);
            return template as DataTemplate;
        }
    }
}

