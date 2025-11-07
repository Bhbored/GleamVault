using GleamVault.MVVM.ViewModels;
using Microsoft.Maui.Controls;
using Shared.Models;
using Syncfusion.Maui.DataSource;
using System.ComponentModel;
using ListSortDirection = Syncfusion.Maui.DataSource.ListSortDirection;
namespace GleamVault.MVVM.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
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
            vm.FilterProductsByCategory();
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
