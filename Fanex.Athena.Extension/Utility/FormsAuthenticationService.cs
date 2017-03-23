namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Web;
    using System.Web.Security;
    using Interfaces;

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            var cache = HttpContext.Current.Response.Cache;
            cache.SetNoStore();
            cache.SetExpires(DateTime.Now.AddMinutes(-1));
            cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}