using GleamVault.MVVM.ViewModels;
using System.ComponentModel;

namespace GleamVault.MVVM.Views;

public partial class InventoryPage : ContentPage
{
	public InventoryPage(InventoryVM vm)
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
        if (BindingContext is InventoryVM vm)
            await vm.LoadDataAsync();
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
}