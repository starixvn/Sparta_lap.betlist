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

            clientToken = request.IsAjaxRequest() ? request.Headers.Get(KEYNAME) : request.Form[KEYNAME];

            if (clientToken == null)
            {
                throw new HttpAntiForgeryException($"Header/Form does not contain {KEYNAME}");
            }

            var serverTokenCookie = request.Cookies.Get(AntiForgeryConfig.CookieName);
            if (serverTokenCookie == null || serverTokenCookie.Value == null)
            {
                throw new HttpAntiForgeryException($"Cookies does not contain {KEYNAME}");
            }

            AntiForgery.Validate(serverTokenCookie.Value, clientToken);
        }
    }
}