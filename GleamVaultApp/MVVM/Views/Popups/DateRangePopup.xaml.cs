using CommunityToolkit.Maui.Views;
using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views.Popups;

public partial class DateRangePopup : Popup
{
    private TransactionVM? _viewModel;

    public DateRangePopup(TransactionVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private async void CloseButton_Clicked(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.TempDateRange = _viewModel.SelectedDateRange;
        }
        await CloseAsync();
    }

    private void ResetButton_Clicked(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.TempDateRange = _viewModel.SelectedDateRange;
        }
    }

    private async void ApplyButton_Clicked(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            if (_viewModel.TempDateRange != null && _viewModel.TempDateRange.StartDate != null && _viewModel.TempDateRange.EndDate != null)
            {
                _viewModel.SelectedDateRange = _viewModel.TempDateRange;
            }
            else
            {
                _viewModel.SelectedDateRange = null;
            }
        }
        await CloseAsync();
    }
}
