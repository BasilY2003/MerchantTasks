using CommonLib.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace CommonLib
{
    public class NHibernateHelper
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateHelper(string connectionString)
        {
            _sessionFactory = CreateSessionFactory(connectionString);
        }

        public ISessionFactory SessionFactory => _sessionFactory;

        public ISession OpenSession() => _sessionFactory.OpenSession();

        private static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return Fluently.Configure()
                .Database(OracleManagedDataClientConfiguration.Oracle10
                    .ConnectionString(connectionString))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<MerchantsMap>())
                .ExposeConfiguration(cfg =>
                {
                    new SchemaUpdate(cfg).Execute(false, true);
                })
                .BuildSessionFactory();
        }
    }
}
