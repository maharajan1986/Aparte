using Aparte.Models;
using System.Data.Entity;

namespace Aparte.Repository
{
    /// <summary>
    /// Context that handles Tenant registration and access permission
    /// </summary>
    public class ApiContext : AparteContextBase
    {
        const string ContextName = "AuthenticationContext";
        public ApiContext() : this(ConnectionString)
        {

        }
        
        public ApiContext(string connection) 
            : base(connection)
        {
            Database.SetInitializer<ApiContext>(null);
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }
        public DbSet<xAttribute> Attributes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().ToTable("tenant.Tenant");
            modelBuilder.Entity<SystemUser>().ToTable("tenant.SystemUser");
            modelBuilder.Entity<TenantUser>().ToTable("tenant.TenantUser");
            modelBuilder.Entity<xAttribute>().ToTable("dbo.Attribute");
        }
    }
}
