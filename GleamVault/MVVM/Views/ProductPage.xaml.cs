using GleamVault.MVVM.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GleamVault.MVVM.Views;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        var selector = Resources["ProductTemplateSelector"] as Converters.ProductTemplateSelector;
        if (selector != null)
        {
            selector.ViewModel = vm;
        }

        vm.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ProductVM.IsDataLoading) && ProductsList != null)
        {
            var currentItems = ProductsList.ItemsSource;
            ProductsList.ItemsSource = null;
            ProductsList.ItemsSource = currentItems;
        }
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
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProductVM vm)
            await vm.LoadDataAsync();
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
            vm.FilterProductsByCategory();
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