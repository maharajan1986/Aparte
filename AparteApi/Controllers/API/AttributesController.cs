namespace AparteApi.Controllers
{
    using Aparte.MasterData;
    using Aparte.Models;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using System.Linq;
    using System.Web.Http;
    public class AttributesController : BaseController<Aparte.Models.xAttribute>
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return base.get();
        }
        
        [EnableQuery, HttpGet]        
        [ODataRoute("GetAttributes(ValueRule={valueRule})")]
        public IQueryable<xAttribute> GetAttributes([FromODataUri] AttributeRule valueRule)
        {            
            var qry = base.DbContext.Attributes.Where(x => x.Rules == valueRule);
            return qry.AsQueryable();
        }
    }
}
