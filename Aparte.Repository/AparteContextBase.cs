using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Aparte.Repository
{
    public abstract class AparteContextBase : DbContext
    {
        public static string ConnectionString = "";
        static AparteContextBase()
        {
            // EF 6.0.2 throws an exception, unless we first probe the provider types.
            var providerType = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            //Do nothing to the existing database schema!!!
            System.Data.Entity.Database.SetInitializer<AparteContextBase>(null);            
        }

        protected AparteContextBase(string connection) : base(connection) { }

        public ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }
    }
}
