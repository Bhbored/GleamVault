using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace GleamVault.Utility;

public static class SnackbarHelper
{
    public static async Task ShowSuccessAsync(
        string message,
        IView? anchor = null,
        string? actionButtonText = null,
        Func<Task>? action = null)
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Color.FromArgb("#28A745"),
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
            CornerRadius = new CornerRadius(12),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14, FontWeight.Bold),
            CharacterSpacing = 0.5
        };

        ISnackbar snackbar;
        
        if (action != null && !string.IsNullOrWhiteSpace(actionButtonText))
        {
            snackbar = Snackbar.Make(
                message,
                async () => await action(),
                actionButtonText,
                TimeSpan.FromSeconds(3),
                snackbarOptions,
                anchor);
        }
        else
        {
            snackbar = Snackbar.Make(
                message,
                () => { },
                string.Empty,
                TimeSpan.FromSeconds(3),
                snackbarOptions,
                anchor);
        }

        await snackbar.Show();
    }

    public static async Task ShowErrorAsync(
        string message,
        IView? anchor = null,
        string? actionButtonText = null,
        Func<Task>? action = null)
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Color.FromArgb("#DC3545"),
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.White,
            CornerRadius = new CornerRadius(12),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14, FontWeight.Bold),
            CharacterSpacing = 0.5
        };

        ISnackbar snackbar;
        
        if (action != null && !string.IsNullOrWhiteSpace(actionButtonText))
        {
            snackbar = Snackbar.Make(
                message,
                async () => await action(),
                actionButtonText,
                TimeSpan.FromSeconds(4),
                snackbarOptions,
                anchor);
        }
        else
        {
            snackbar = Snackbar.Make(
                message,
                () => { },
                string.Empty,
                TimeSpan.FromSeconds(4),
                snackbarOptions,
                anchor);
        }

        await snackbar.Show();
    }
}
