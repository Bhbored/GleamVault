using CommunityToolkit.Maui.Views;
using Shared.Models;

namespace GleamVault.MVVM.Views.Popups;

public partial class ProductDetailsPopup : Popup
{
    public ProductDetailsPopup(Product product)
    {
        InitializeComponent();
        BindingContext = product;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}

