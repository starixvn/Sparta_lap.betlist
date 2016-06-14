namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Helpers;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CustomValidateAntiForgery : FilterAttribute, IAuthorizationFilter
    {
        private const string KEY_NAME = "__RequestVerificationToken";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            string clientToken = null;
            if (request.IsAjaxRequest())
            {
                clientToken = request.Headers.Get(KEY_NAME);
            }
            else
            {
                clientToken = request.Form[KEY_NAME];
            }

            if (clientToken == null)
            {
                throw new HttpAntiForgeryException(string.Format("Header/Form does not contain {0}", KEY_NAME));
            }

            string serverToken = filterContext.HttpContext.Request.Cookies.Get(KEY_NAME).Value;
            if (serverToken == null)
            {
                throw new HttpAntiForgeryException(string.Format("Cookies does not contain {0}" + KEY_NAME));
            }

            AntiForgery.Validate(serverToken, clientToken);
        }
    }
}