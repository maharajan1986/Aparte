using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Microsoft.AspNet.OData.Builder;
using System.Web.Http;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData;
using Aparte.Models;
using Microsoft.AspNet.OData.Routing;
using AparteApi.Controllers;
using Microsoft.AspNet.OData.Query;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Aparte.Repository.Test.OdataControllerTests.TenantController
{
    [TestClass]
    public class TenantControllerTest
    {
        public TenantControllerTest()
        {
            ApiContext.ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Tenant;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
        [TestMethod]
        public async Task TenantGet()
        {
            //Mock obujects

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:58578/aparte/Tenant");

            ODataModelBuilder builder = Aparte.ModelBuilder.AparteModelBuilder.GetOdataModelBuilder();            
            var model = builder.GetEdmModel();

            HttpRouteCollection routes = new HttpRouteCollection();
            HttpConfiguration config = new HttpConfiguration(routes) { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
        

            config.MapODataServiceRoute("ODataV4Route", "aparte", model);
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            config.EnableDependencyInjection();
            //Check all the routes are properly initialized.
            config.EnsureInitialized();

            request.SetConfiguration(config);
            ODataQueryContext context = new ODataQueryContext(
                model,
                typeof(Tenant),
                new ODataPath(
                    new Microsoft.OData.UriParser.EntitySetSegment(
                        model.EntityContainer.FindEntitySet("Tenants"))
                )
            );


            var controller = new TenantsController();
            controller.Request = request;

            var options = new ODataQueryOptions<Tenant>(context, request);
            var response = await (controller.Get() as IHttpActionResult).ExecuteAsync(CancellationToken.None);
            
            List<Tenant> tenants = await response.Content.ReadAsAsync<List<Tenant>>();
            Assert.IsTrue(tenants.Any());

        }
    }
}
