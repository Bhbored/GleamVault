using CommunityToolkit.Maui;
using GleamVault.Services;
using GleamVault.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
namespace GleamVaultApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options => options.SetShouldEnableSnackbarOnWindows(true))
                .ConfigureSyncfusionToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Light.ttf", "NexaLight");
                    fonts.AddFont("Poppins-Bold.ttf", "NexaHeavy");
                });



#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.RegisterDependencies();
            return builder.Build();
        }
    }
}
