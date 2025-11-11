using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views.Popups;

public partial class ReceiptPopup : Popup
{
	public ReceiptPopup(HomeVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (BindingContext is HomeVM vm)
		{
			await CloseAsync();
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if (BindingContext is HomeVM vm)
        {
            await vm.CompleteSaleConfirmationAsync();
            await CloseAsync();
        }
    }
}

