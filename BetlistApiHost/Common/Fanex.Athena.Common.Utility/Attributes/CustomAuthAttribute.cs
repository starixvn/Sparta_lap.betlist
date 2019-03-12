using System;
using System.Configuration;
using System.Web.Mvc;

namespace Fanex.Athena.Common.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CustomAuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KEYNAME = "BETLIST_TOKEN_KEY";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var token = request.Headers.Get(KEYNAME);
            if (token != ConfigurationManager.AppSettings["BetListApiTokenValue"])
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
        }
    }
}