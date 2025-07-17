using CommonLib.Localization;
using CommonLib.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLib
{
    public static class CommonLibSetup
    {
        public static IServiceCollection AddCommonLib(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<LoggingService>();

            services.Scan(scan => scan
                .FromAssemblyOf<Services.MerchantGroupService>() 
                .AddClasses(classes => classes.InNamespaces("CommonLib.Services"))
                .AsSelf()
                .WithScopedLifetime()
            );

            services.AddLocalization();

            return services;
        }
    }
}
