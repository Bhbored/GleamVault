using GleamVault.MVVM.ViewModels;
using System.ComponentModel;
namespace GleamVault.MVVM.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.PropertyChanged += ViewModel_PropertyChanged;
    }
    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ProductVM.IsDataLoading) && ProductsList != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var currentItems = ProductsList.ItemsSource;
                if (currentItems != null)
                {
                    ProductsList.ItemsSource = null;
                    ProductsList.ItemsSource = currentItems;
                }
            });
        }
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomeVM vm)
            await vm.LoadDataAsync();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (ProductGridLayout == null) return;
        var span = width switch
        {
            < 900 => 2,
            < 1300 => 3,
            _ => 4
        };
        if (ProductGridLayout.SpanCount != span)
            ProductGridLayout.SpanCount = span;

        if (ShimmerGridLayout != null && ShimmerGridLayout.SpanCount != span)
            ShimmerGridLayout.SpanCount = span;
    }
    private void autocomplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is HomeVM vm)
        {
            vm.FilterProducts();
        }
    }

    private void CategoryCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is HomeVM vm)
        {
            vm.FilterProductsByCategory();
        }
    }

    private void HallmarkCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is HomeVM vm)
        {
            vm.FilterProductsByHallmark();
        }
    }

    private void SortSeg_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if (BindingContext is HomeVM vm)
        {
            vm.SortNow();
        }

    }
}