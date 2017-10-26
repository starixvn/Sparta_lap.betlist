namespace Fanex.Athena.Service.Core.Helpers
{
    using System.IO;
    using System.Web;
    using Fanex.Athena.Model.Core.DataDomain;

    public static class ClientViewHelper
    {
        public static string GetViewPath(string viewPath, ClientViewType viewtype)
        {
            var themeSuffix = viewtype == ClientViewType.Desktop ? "Desktop" : string.Empty;

            if (string.IsNullOrEmpty(themeSuffix) || string.IsNullOrEmpty(viewPath))
            {
                return viewPath;
            }

            var newViewPath = viewPath.Replace("Views", "Views/" + themeSuffix);
            return File.Exists(HttpContext.Current.Server.MapPath(newViewPath)) ? newViewPath : viewPath;
        }
    }
}