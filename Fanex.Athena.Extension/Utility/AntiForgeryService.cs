using System.Web.Helpers;

namespace Fanex.Athena.Extension.Utility
{
    public class AntiForgeryService : IAntiForgeryService
    {
        public void Validate()
        {
            AntiForgery.Validate();
        }

        public void Validate(string cookieToken, string formToken)
        {
            AntiForgery.Validate(cookieToken, formToken);
        }
    }
}