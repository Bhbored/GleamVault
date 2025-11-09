using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.ViewModels;
using Shared.Models;
using Shared.Models.Enums;

namespace GleamVault.MVVM.Views.Popups;

public partial class AddProductPopup : Popup
{
    public AddProductPopup(ProductVM viewModel)
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
        if (BindingContext is not ProductVM vm) return;

        var name = NameEntry.Text?.Trim() ?? string.Empty;
        var description = DescriptionEditor.Text?.Trim();
        var category = CategoryCombo.SelectedItem as Category;
        var hallmark = HallmarkPicker.SelectedItem as HallmarkType?;
        var unitCost = float.TryParse(UnitCostEntry.Text, out var cost) ? cost : 0f;
        var unitPrice = float.TryParse(UnitPriceEntry.Text, out var price) ? price : 0f;
        var imageUrl = ImageUrlEntry.Text?.Trim();
        var weightUnit = WeightUnitPicker.SelectedItem as WeightUnit? ?? WeightUnit.Grams;
        var weight = float.TryParse(WeightEntry.Text, out var w) ? w : 0f;
        var isUniquePiece = IsUniquePieceSwitch.IsToggled;

        await vm.AddProductAsync(
            name,
            description,
            category,
            hallmark,
            unitCost,
            unitPrice,
            imageUrl,
            weightUnit,
            weight,
            isUniquePiece
        );

        await CloseAsync();
    }
}

