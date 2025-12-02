using GleamVault.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Licensing;

namespace GleamVaultApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfcnRTRGBYUUN/V0ZWYEg=");
            _serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var sessionService = _serviceProvider.GetRequiredService<ISessionService>();
            return new Window(new AppShell(sessionService));
        }
    }
}