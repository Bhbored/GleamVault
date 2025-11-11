using GleamVault.MVVM.ViewModels;
using GleamVault.MVVM.Views;
using GleamVault.Services.Interfaces;


namespace GleamVault.Services
{
    public static class DIContainer
    {


        public static IServiceCollection RegisterGoldPriceService(this IServiceCollection services)
        {
            services.AddSingleton<IGoldPriceService, GoldPriceService>();
            return services;
        }
        public static IServiceCollection RegisterAuthServices(this IServiceCollection services)
        {
            services.AddSingleton<IAdvanceHttpService, HttpService>();
            return services;
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddTransient<HomeVM>();
            services.AddTransient<TransactionVM>();
            services.AddTransient<ProductVM>();
            services.AddTransient<ReportsVM>();
            services.AddTransient<InventoryVM>();
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
                    .RegisterAuthServices()
                    .RegisterGoldPriceService()
                    .RegisterViews()
                    .RegisterViewModels();
        }
    }
}
