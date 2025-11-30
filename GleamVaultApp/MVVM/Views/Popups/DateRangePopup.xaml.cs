using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views.Popups;

public partial class DateRangePopup : Popup
{
    public DateRangePopup(TransactionVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void CloseButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync();
    }
}

