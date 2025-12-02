using CommunityToolkit.Maui.Extensions;
using GleamVault.MVVM.Views;
using GleamVault.MVVM.Views.Popups;
using GleamVault.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace GleamVaultApp
{
    public partial class AppShell : Shell
    {
        private readonly ISessionService _sessionService;

        public AppShell(ISessionService sessionService)
        {
            InitializeComponent();
            _sessionService = sessionService;
            Routing.RegisterRoute("LogoutRoute", typeof(LogoutPopup));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));

            Loaded += OnShellLoaded;
        }

        private async void OnShellLoaded(object? sender, EventArgs e)
        {
            await Current.GoToAsync($"///: ////{nameof(LoadingPage)}");
        }

        public void SetFlyoutItemsVisibility(bool isAdmin)
        {
            if (InventoryFlyoutItem != null)
            {
                InventoryFlyoutItem.IsVisible = isAdmin;
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Current.ShowPopupAsync(new LogoutPopup(_sessionService));
        }
    }
}
