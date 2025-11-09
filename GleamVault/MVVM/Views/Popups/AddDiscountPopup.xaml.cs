using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views.Popups;

public partial class AddDiscountPopup : CommunityToolkit.Maui.Views.Popup
{
    public AddDiscountPopup(ProductVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void CloseButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync();
    }

    private void autocomplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProducts();
        }
    }
}

