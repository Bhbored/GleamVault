using GleamVault.MVVM.ViewModels;
using GleamVault.MVVM.Views;
using GleamVault.Services.Interfaces;
using Shared.Contracts;


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
            services.AddSingleton<IShopDataStore, ShopDataStore>();
            services.AddSingleton<IAdvanceHttpService, HttpService>();
            services.AddSingleton<ISessionService, SessionService>();
            return services;
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddTransient<HomeVM>();
            services.AddTransient<TransactionVM>();
            services.AddTransient<ProductVM>();
            services.AddTransient<DiscountVM>();
            services.AddTransient<ReportsVM>();
            services.AddTransient<InventoryVM>();
            services.AddTransient<CustomerVM>();
            services.AddTransient<LoginVM>();
            services.AddTransient<SignUpVM>();
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
            services.AddTransient<LoginPage>();
            services.AddTransient<SignUpPage>();
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
