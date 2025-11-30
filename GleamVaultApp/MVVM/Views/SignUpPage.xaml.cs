using GleamVault.MVVM.ViewModels;
using Microsoft.Maui.Controls;

namespace GleamVault.MVVM.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}