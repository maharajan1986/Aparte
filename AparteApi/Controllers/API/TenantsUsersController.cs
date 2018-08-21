using Aparte.Models;
using System.Web.Http;

namespace AparteApi.Controllers
{
    public class TenantUsersController : BaseController<TenantUser>
    {
        public IHttpActionResult Get()
        {
            return base.get();
        }
    }
}
