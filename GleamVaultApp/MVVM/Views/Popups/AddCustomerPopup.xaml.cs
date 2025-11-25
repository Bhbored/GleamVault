using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views.Popups;

public partial class AddCustomerPopup : Popup
{
    public AddCustomerPopup(CustomerVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void CancelButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void SaveButton_Clicked(object? sender, EventArgs e)
    {
        if (BindingContext is not CustomerVM vm) return;

        var fullName = FullNameEntry.Text?.Trim() ?? string.Empty;
        var phoneNumber = PhoneNumberEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var address = AddressEditor.Text?.Trim();
        var dateOfBirth = DateOfBirthPicker.Date;
        var loyaltyPoints = int.TryParse(LoyaltyPointsEntry.Text, out var points) ? points : 0;
        var notes = NotesEditor.Text?.Trim();

        await vm.AddNewCustomerAsync(
            fullName,
            phoneNumber,
            email,
            address,
            dateOfBirth,
            loyaltyPoints,
            notes
        );

        await CloseAsync();
    }
}
