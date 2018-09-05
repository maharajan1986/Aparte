using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aparte.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Aparte.Repository.Test.TenantTests
{
    [TestClass]
    public class TestTenant : IDisposable, ICRUDTest
    {
        private ApiContext dbContext = null;
        public TestTenant()
        {
            ApiContext.ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Tenant;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            dbContext = new ApiContext();
        }
        [TestMethod]
        public void Get()
        {
            //Arrange
            var tenant = dbContext.Tenants.FirstOrDefault();
           
            //Assert
            Assert.IsNotNull(tenant.PK);
        }
        [TestMethod]
        public void Add()
        {
            //Arrange
            var tenant = new Tenant() { Name = "Tenant1" };

            //Act
            dbContext.Tenants.Add(tenant);
            dbContext.SaveChanges();
            //Assert
            Assert.IsNotNull(tenant.PK);
        }
        [TestMethod]
        public void Delete()
        {
            string testName = "Tenant1";
            
            var testTenants = dbContext.Tenants.Where(x => x.Name == testName);
            dbContext.Tenants.RemoveRange(testTenants);
            dbContext.SaveChanges();

            var isExists = dbContext.Tenants.Where(x => x.Name == testName).Any();
            Assert.IsFalse(isExists);
        }              
   
        [TestMethod]
        public void Update()
        {
           
        }
        public void Dispose()
        {
            dbContext = null;
        }
    }
}
