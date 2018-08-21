using Aparte.AuthServer;
using Aparte.Models;
using Aparte.Repository;
using Microsoft.AspNet.OData;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace AparteApi.Controllers
{
    public abstract class BaseController<T> : ODataController where T : Base
    {
        protected ApiContext DbContext = new ApiContext();
        [EnableQuery]
        protected virtual IHttpActionResult get()
        {
            var list = DbContext.Set<T>().AsQueryable();
            return Ok(list);
        }

        protected virtual HttpResponseMessage post(T obj)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage));
                return AuthResponse.CreateErrorResponse(HttpStatusCode.BadRequest, message, Request);
            }
            else
            {
                DbContext.Set<T>().Add(obj);
                DbContext.SaveChanges();
            }
               
            return CreateODataResponse(HttpStatusCode.OK, obj);            
        }

        protected HttpResponseMessage CreateODataResponse(HttpStatusCode status, T obj)
        {
            var response = Request.CreateResponse<T>(status, obj);
            response.Headers.Location = Request.RequestUri;
            return response;
        }
    }
}
