using Aparte.MasterData;
using Aparte.Models;
using Microsoft.AspNet.OData.Builder;

namespace Aparte.ModelBuilder
{
    public abstract class AparteModelBuilder
    {
        public static ODataConventionModelBuilder GetOdataModelBuilder()
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
