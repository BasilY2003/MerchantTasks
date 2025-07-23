using CommonLib.Interfaces;
using CommonLib.Middlewares;
using CommonLib.Pdf;
using CommonLib.Services;
using CommonLib.Utils;
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
                .FromAssemblyOf<JwtService>() 
                .AddClasses(classes => classes.InNamespaces("CommonLib.Services"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
            services.AddScoped<IPasswordHasher,PasswordHasher>();
            services.AddLocalization();

            return services;
        }
    }
}
