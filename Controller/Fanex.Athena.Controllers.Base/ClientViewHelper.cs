using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Service.Core.Users;

namespace Fanex.Athena.Controllers
{
    public static class ClientViewHelper
    {
        private static readonly IDictionary<ClientViewType, string> ClientViewTypes =
            new Dictionary<ClientViewType, string>() { { ClientViewType.Default, string.Empty }, { ClientViewType.Desktop, "Desktop" } };

        public static string GetViewPath(string viewPath, ClientViewType viewtype)
        {

            var themeSuffix = string.Empty;
            ClientViewTypes.TryGetValue(viewtype, out themeSuffix);

            if (string.IsNullOrEmpty(themeSuffix) || string.IsNullOrEmpty(viewPath))
            {
                return viewPath;
            }

            var newViewPath = viewPath.Replace("Views", "Views/" + themeSuffix);
            return File.Exists(HttpContext.Current.Server.MapPath(newViewPath)) ? newViewPath : viewPath;
        }
    }
}