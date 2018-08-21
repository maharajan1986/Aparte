using Aparte.Models;
using System.Net.Http;
using System.Web.Http;

namespace AparteApi.Controllers
{
    public class TenantsController : BaseController<Tenant>
    {
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
