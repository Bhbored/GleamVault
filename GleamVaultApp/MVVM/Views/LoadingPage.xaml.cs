using GleamVault.Services.Interfaces;
using Shared.Models.Enums;
using System;

namespace GleamVault.MVVM.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckSessionAndNavigate();
    }

    private async Task CheckSessionAndNavigate()
    {
        await Task.Delay(500);

        if (Handler?.MauiContext?.Services == null)
        {
            await Task.Delay(200);
        }

        var sessionService = Handler?.MauiContext?.Services?.GetService<ISessionService>();
        if (sessionService == null) return;

        var session = await sessionService.GetSessionAsync();

        if (session == null)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

        bool isAdmin = session.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                      session.Role.Equals(UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);

        if (Shell.Current is GleamVaultApp.AppShell appShell)
        {
            appShell.SetFlyoutItemsVisibility(isAdmin);
        }

        await Shell.Current.GoToAsync("//HomePage");
    }
}
