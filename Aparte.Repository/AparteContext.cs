using Aparte.Models;
using Aparte.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Repository
{
    public class AparteContext : DbContext
    {
        const string ContextName = "AparteContext";
        public AparteContext() : this(ConnectionString)
        {

        }
        public static string ConnectionString = "";
        public AparteContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<AparteContext>(null);
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().ToTable("dbo.Tenant");
        }
    }
}
