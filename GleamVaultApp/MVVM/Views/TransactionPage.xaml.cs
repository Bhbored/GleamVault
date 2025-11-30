using GleamVault.MVVM.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.ComponentModel;

namespace GleamVault.MVVM.Views;

public partial class TransactionPage : ContentPage
{
	private bool _isDataLoaded = false;

	public TransactionPage(TransactionVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.PropertyChanged += ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(TransactionVM.ShimmerNotLoading))
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (TransactionsList != null)
				{
					var items = TransactionsList.ItemsSource;
					TransactionsList.ItemsSource = null;
					TransactionsList.ItemsSource = items;
				}
			});
		}
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is TransactionVM vm && !_isDataLoaded)
		{
			_isDataLoaded = true;
			await vm.LoadDataAsync();
		}
	}

	private async void Transaction_Tapped(object? sender, EventArgs e)
	{
		if (sender is Border border && border.BindingContext is Shared.Models.Transaction transaction)
		{
			if (BindingContext is TransactionVM vm)
			{
				await vm.ShowTransactionDetailsAsync(transaction);
			}
		}
	}

}
