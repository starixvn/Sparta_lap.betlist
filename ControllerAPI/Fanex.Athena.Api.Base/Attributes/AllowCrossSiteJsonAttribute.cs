namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                NoCache = true,
                NoStore = true
            };

            actionContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var rqstMethod = actionContext.Request.Headers.GetValues("Access-Control-Request-Method").FirstOrDefault();
            if (rqstMethod == "OPTIONS" || rqstMethod == "POST")
            {
                actionContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                actionContext.Response.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With, Accept, Access-Control-Allow-Origin, Content-Type");
            }
           
            base.OnActionExecuting(actionContext);
        }
    }
}