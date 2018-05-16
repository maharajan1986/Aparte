using Aparte.Repository.Interfaces;
using Aparte.Models;
using System.Data.Entity;

namespace Aparte.Repository.Test
{
    public class TestAparteContext : ITenantContext
    {
        public TestAparteContext()
        {
            this.Tenants = new TestProductDbSet();
        }
        public DbSet<Tenant> Tenants { get; set; }

        public void MarkAsModified(Tenant item) { }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
