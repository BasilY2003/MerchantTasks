using CommonLib.Localization;
using CommonLib.Middlewares;
using CommonLib.Pdf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLib
{
    public static class CommonLibSetup
    {
        public static IServiceCollection AddCommonLib(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<LoggingService>();
            services.AddSingleton<PdfGenerator>();


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
