using GleamVault.MVVM.ViewModels;

namespace GleamVault.MVVM.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}