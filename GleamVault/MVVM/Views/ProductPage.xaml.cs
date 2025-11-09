using GleamVault.MVVM.ViewModels;
using Microsoft.Maui.ApplicationModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GleamVault.MVVM.Views;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (ProductGridLayout == null) return;
        var span = width switch
        {
            < 900 => 2,
            < 1300 => 3,
            _ => 3
        };
        if (ProductGridLayout.SpanCount != span)
            ProductGridLayout.SpanCount = span;

        if (ShimmerGridLayout != null && ShimmerGridLayout.SpanCount != span)
            ShimmerGridLayout.SpanCount = span;
    }
    private bool _isDataLoaded = false;
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProductVM vm && !_isDataLoaded)
        {
            _isDataLoaded = true;
            await vm.LoadDataAsync();
        }
    }

    private void autocomplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProducts();
        }
    }

    private void CategoryCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProductsByCategory();
        }
    }

    private void HallmarkCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProductsByHallmark();
        }
    }

    private void SortSeg_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.SortNow();
        }

    }
}