using CommonLib;
using DataLib.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

namespace DataLib
{
    public static class DataLibSetup
    {
        public static IServiceCollection AddDataLib(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("OracleDb");
            var helper = new NHibernateHelper(connectionString);

            services.AddSingleton(helper);
            services.AddSingleton(helper.SessionFactory);
            services.AddScoped<ISession>(sp => helper.SessionFactory.OpenSession());

            services.Scan(scan => scan
             .FromAssemblyOf<MerchantRepository>()
             .AddClasses(classes => classes.InNamespaces("DataLib.Repository"))
             .AsSelf()
             .WithScopedLifetime()
         );

            return services;
        }
    }

}
