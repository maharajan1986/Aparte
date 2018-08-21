using Aparte.AuthServer;
using Aparte.MasterData;
using Aparte.Models;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace AparteApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new SecurityHandler());            
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            //Configure Odata service
            ODataBatchHandler odataBatchHandler = new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer);
            odataBatchHandler.MessageQuotas.MaxOperationsPerChangeset = 1200;
            odataBatchHandler.MessageQuotas.MaxPartsPerBatch = 2;

            //OData Route Configurations
            var builder = GetOdataModelBuilder();            
            config.MapODataServiceRoute(
                routeName: "ODataV4Route",
                routePrefix: "aparte",
                model: builder.GetEdmModel(),
                batchHandler: odataBatchHandler
                );

            //Route template to route api controllers.
            config.Routes.MapHttpRoute
                (
                name: "DefaultApi",
                 routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute
                (
                name: "CatchAllRoutes",
                routeTemplate: "{*url}" // to evaluate routeTemplate in Security Handler
                );

            config.EnsureInitialized();
        }

        private static ODataConventionModelBuilder GetOdataModelBuilder()
        {
            var builder = new ODataConventionModelBuilder();
            
            builder.Namespace = "ODataService";
            #region Tenant            
            builder.EntitySet<Tenant>("Tenants");
            builder.EntitySet<SystemUser>("SystemUsers");
            builder.EntitySet<TenantUser>("TenantUsers");
            builder.EntitySet<xAttribute>("Attributes");

            //var function = builder.Function("GetAttributes");
            //function.Parameter<AttributeRule>("ValueRule");
            //function.ReturnsCollectionFromEntitySet<xAttribute>("Attributes");

            // Unbound function
            var function = builder.Function("GetAttributes");
            function.Parameter<AttributeRule>("ValueRule");
            function.ReturnsCollectionFromEntitySet<xAttribute>("Attributes");
            #endregion
            builder.RemoveEnumType(typeof(AttributeRule));

            return builder;
        }
    }
}
