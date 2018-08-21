using Aparte.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AparteApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static string DB_AUTHENTICATION = "AparteAuthentication";
        private static string DB_API = "AparteApi";
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SetDatabase();
        }
        private static void SetDatabase()
        {
            //To stop database schema compare.
            System.Data.Entity.Database.SetInitializer<ApiContext>(null);

            var cnnName = System.Configuration.ConfigurationManager.AppSettings[DB_API];
            var cnn = System.Configuration.ConfigurationManager.ConnectionStrings[cnnName];
            ApiContext.ConnectionString = cnn.ConnectionString;
        }
    }
}
