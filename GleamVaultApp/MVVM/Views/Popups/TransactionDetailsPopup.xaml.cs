using CommunityToolkit.Maui.Views;

namespace GleamVault.MVVM.Views.Popups;

public partial class TransactionDetailsPopup : Popup
{
    public TransactionDetailsPopup(Shared.Models.Transaction transaction)
    {
        InitializeComponent();
        BindingContext = transaction;
    }

    private void CloseButton_Clicked(object? sender, EventArgs e)
    {
        CloseAsync();
    }
}

