using GleamVault.MVVM.ViewModels;
using GleamVault.Utility;
using System.ComponentModel;

namespace GleamVault.MVVM.Views;

public partial class CustomerPage : ContentPage
{
    public CustomerPage Current { get; private set; }

    public CustomerPage(CustomerVM vm)
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
        if (e.PropertyName == nameof(CustomerVM.ShimmerNotLoading) && CustomersList != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var currentItems = CustomersList.ItemsSource;
                if (currentItems != null)
                {
                    CustomersList.ItemsSource = null;
                    CustomersList.ItemsSource = currentItems;
                }
            });
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CustomerVM vm)
        {
            if (vm.AllCustomers.Count > 0)
            {
                return;
            }
            else
            {
                await vm.LoadDataAsync();

            }
        }
    }

    private void autocomplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is CustomerVM vm)
        {
            vm.FilterCustomers();
        }
    }

    private void SortSeg_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if (BindingContext is CustomerVM vm)
        {
            vm.SortNow();
        }
    }

    private async Task ShowDeleteSnackbarAsync(string customerName, Func<Task> undoAction)
    {
        await SnackbarHelper.ShowErrorAsync(
            $"'{customerName}' deleted",
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