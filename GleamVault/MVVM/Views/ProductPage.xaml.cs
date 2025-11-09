using GleamVault.MVVM.ViewModels;
using System.Threading.Tasks;

namespace GleamVault.MVVM.Views;

public partial class ProductPage : ContentPage
{
	public ProductPage(ProductVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProductVM vm)
            await vm.LoadDataAsync();
    }

    private void autocomplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProducts();
        }
    }

    private void CategoryCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProductsByCategory();
        }
    }

    private void HallmarkCombo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.FilterProductsByCategory();
        }
    }

    private void SortSeg_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductVM vm)
        {
            vm.SortNow();
        }

    }
}