using Aparte.Models;
using Microsoft.AspNet.OData;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace AparteApi.Controllers
{
    public class TenantsController : BaseController<Tenant>
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return base.get();
        }
        public HttpResponseMessage Post(Tenant t)
        {
            return base.post(t);
        }       
    }
}
