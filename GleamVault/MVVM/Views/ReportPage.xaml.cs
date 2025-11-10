using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views;

public partial class ReportPage : ContentPage
{
	private bool _isDataLoaded = false;

	public ReportPage(ReportsVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is ReportsVM vm && !_isDataLoaded)
		{
			_isDataLoaded = true;
			await vm.LoadDataAsync();
		}
	}

	private void TimeframeCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
	{
		if (BindingContext is ReportsVM vm && e.AddedItems.Count > 0 && e.AddedItems[0] is string timeframe)
		{
			vm.SelectedTimeframe = timeframe;
		}
	}
}