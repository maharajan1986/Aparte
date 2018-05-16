using Aparte.Models;
using System.Data.Entity;

namespace Aparte.Repository.Interfaces
{
    public interface ITenantContext
    {
        DbSet<Tenant> Tenants { get; }
        int SaveChanges();
        void MarkAsModified(Tenant item);
    }
}
