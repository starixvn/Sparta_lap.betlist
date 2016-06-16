namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Helpers;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CustomValidateAntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KEYNAME = "__RequestVerificationToken";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            string clientToken = null;
            if (request.IsAjaxRequest())
            {
                clientToken = request.Headers.Get(KEYNAME);
            }
            else
            {
                clientToken = request.Form[KEYNAME];
            }

            if (clientToken == null)
            {
                throw new HttpAntiForgeryException(string.Format("Header/Form does not contain {0}", KEYNAME));
            }

            string serverToken = filterContext.HttpContext.Request.Cookies.Get(KEYNAME).Value;
            if (serverToken == null)
            {
                throw new HttpAntiForgeryException(string.Format("Cookies does not contain {0}" + KEYNAME));
            }

            AntiForgery.Validate(serverToken, clientToken);
        }
    }
}