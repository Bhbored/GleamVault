using GleamVault.MVVM.ViewModels;
using GleamVault.MVVM.Views;
using GleamVault.Services.Interfaces;


namespace GleamVault.Services
{
    public static class DIContainer
    {
        public static IServiceCollection RegisterImageServices(this IServiceCollection services)
        {

            services.AddSingleton<IImageService, MockImageService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<CleanupService>();
            return services;
        }
        public static IServiceCollection RegisterAuthServices(this IServiceCollection services)
        {

            return services;
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddTransient<HomeVM>();
            services.AddTransient<TransactionVM>();
            services.AddTransient<ProductVM>();
            return services;
        }

        public static IServiceCollection RegisterViews(this IServiceCollection services)
        {
            services.AddTransient<HomePage>();
            services.AddTransient<TransactionPage>();
            services.AddTransient<ProductPage>();
            services.AddTransient<DiscountPage>();
            services.AddTransient<CustomerPage>();
            services.AddTransient<InventoryPage>();
            services.AddTransient<ReportPage>();
            return services;
        }

        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            return services
                    .RegisterImageServices()
                    .RegisterViews()
                    .RegisterViewModels();
        }
    }
}
