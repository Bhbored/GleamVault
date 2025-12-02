using CommunityToolkit.Maui.Views;
using GleamVault.Services.Interfaces;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace GleamVault.MVVM.Views.Popups;

public partial class LogoutPopup : Popup
{
    private readonly ISessionService _sessionService;

    public LogoutPopup(ISessionService sessionService)
    {
        InitializeComponent();
        _sessionService = sessionService;
    }

    private async void CancelButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void LogoutButton_Clicked(object? sender, EventArgs e)
    {
        bool confirmed = await Shell.Current.DisplayAlert(
            "Confirm Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No"
        );

        if (!confirmed)
        {
            return;
        }

        try
        {
            LogoutActivityIndicator.IsVisible = true;
            LogoutActivityIndicator.IsRunning = true;
            ContentStack.IsVisible = false;
            LogoutButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            await _sessionService.ClearSessionAsync();

            await CloseAsync();

            await Shell.Current.GoToAsync("//LoginPage");
        }
        catch (Exception ex)
        {
            LogoutActivityIndicator.IsVisible = false;
            LogoutActivityIndicator.IsRunning = false;
            ContentStack.IsVisible = true;
            LogoutButton.IsEnabled = true;
            CancelButton.IsEnabled = true;

            Debug.WriteLine($"Logout error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "An error occurred during logout. Please try again.", "OK");
        }
    }
}
