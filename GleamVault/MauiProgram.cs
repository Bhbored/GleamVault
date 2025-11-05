using CommunityToolkit.Maui;
using GleamVault.Services;
using Microsoft.Extensions.Logging;

namespace GleamVault
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Nexa-ExtraLight.ttf", "NexaLight");
                    fonts.AddFont("Nexa-Heavy.ttf", "NexaHeavy");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.RegisterDependencies();
            return builder.Build();
        }
    }
}
