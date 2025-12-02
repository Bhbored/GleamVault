using GleamVault.MVVM.ViewModels;
using GleamVault.Utility;
using System.ComponentModel;

namespace GleamVault.MVVM.Views;

public partial class InventoryPage : ContentPage
{
    public InventoryPage Current { get; private set; }

    public InventoryPage(InventoryVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.PropertyChanged += ViewModel_PropertyChanged;
        vm.ShowDeleteSnackbar = ShowDeleteSnackbarAsync;
        vm.ShowSuccessSnackbar = ShowSuccessSnackbarAsync;
        Current = this;
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
        if (BindingContext is InventoryVM vm)
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
            _ => 2
        };
        if (ProductGridLayout.SpanCount != span)
            ProductGridLayout.SpanCount = span;

        if (ShimmerGridLayout != null && ShimmerGridLayout.SpanCount != span)
            ShimmerGridLayout.SpanCount = span;
    }

    private void autocomplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is InventoryVM vm)
        {
            vm.FilterProducts();
        }
    }

    private void CategoryCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is InventoryVM vm)
        {
            vm.FilterProductsByCategory();
        }
    }

    private void HallmarkCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is InventoryVM vm)
        {
            vm.FilterProductsByHallmark();
        }
    }

    private void SortSeg_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if (BindingContext is InventoryVM vm)
        {
            vm.SortNow();
        }
    }

    private async Task ShowDeleteSnackbarAsync(string productName, Func<Task> undoAction)
    {
        await SnackbarHelper.ShowErrorAsync(
            $"'{productName}' deleted",
            Current,
            "UNDO",
            undoAction);
    }

    private async Task ShowSuccessSnackbarAsync(string message)
    {
        await SnackbarHelper.ShowSuccessAsync(
            message,
            Current);
    }
}