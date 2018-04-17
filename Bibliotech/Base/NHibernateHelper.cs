using Bibliotech.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Bibliotech.Base
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                  .ConnectionString(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Bibliotech;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                              .ShowSql()
                )
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Usuario>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                                                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}