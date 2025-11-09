using Syncfusion.Licensing;

namespace GleamVault
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfcnRTRGBYUUN/V0ZWYEg=");
            _serviceProvider = serviceProvider;
            InitializeImageStorage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
        private void InitializeImageStorage()
        {
            var imagesPath = Path.Combine(FileSystem.AppDataDirectory, "images");
            if (!Directory.Exists(imagesPath))
                Directory.CreateDirectory(imagesPath);

            var mockImagesPath = Path.Combine(FileSystem.AppDataDirectory, "mock_images");
            if (!Directory.Exists(mockImagesPath))
                Directory.CreateDirectory(mockImagesPath);
        }
    }
}