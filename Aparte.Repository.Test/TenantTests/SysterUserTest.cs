using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aparte.Models;
using System.Linq;

namespace Aparte.Repository.Test.TenantTests
{
    [TestClass]
    public class SystemUserTest : ICRUDTest
    {
        private ApiContext dbContext = null;
        public SystemUserTest()
        {
            ApiContext.ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Tenant;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            dbContext = new ApiContext();
        }
        [TestMethod]
        public void Add()
        {
            //Arrange
            var systemUser = new SystemUser() { Code = "maharajanmca2010@gmail.com", Name = "Maharajan" };

            //Act
            dbContext.SystemUsers.Add(systemUser);
            dbContext.SaveChanges();
            //Assert
            Assert.IsNotNull(systemUser.PK);
        }

        [TestMethod]
        public void Delete()
        {
            string testName = "Maharajan";

            var testUser = dbContext.SystemUsers.Where(x => x.Name == testName);
            dbContext.SystemUsers.RemoveRange(testUser);
            dbContext.SaveChanges();

            var isExists = dbContext.SystemUsers.Where(x => x.Name == testName).Any();
            Assert.IsFalse(isExists);
        }
        [TestMethod]
        public void Update()
        {
            
        }
    }
}
