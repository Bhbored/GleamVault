using GleamVault.MVVM.ViewModels;
using Shared.Models;
using Microsoft.Maui.Controls;

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
    
    
}