namespace Fanex.Athena.Extension.Utility
{
    using System.Web.Security;
    using Interfaces;

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
    }
}