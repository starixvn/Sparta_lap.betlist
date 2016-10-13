using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Service.Core.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Fanex.Athena.Controllers
{
    public static class ClientViewHelper
    {
        private static readonly IDictionary<ClientViewType, string> _clientViewTypes = new Dictionary<ClientViewType, string>() { { ClientViewType.Default, string.Empty }, { ClientViewType.Desktop, "Desktop" } };

        public static string GetViewPath(string viewPath)
        {
            var sessionService = new SessionService();
            var userSession = sessionService.UserSession;
            var option =

                userSession != null && userSession.viewType != ClientViewType.None ? userSession.viewType :

                (ClientViewType)int.Parse(ConfigurationManager.AppSettings["ClientViewType"].ToString());


            var themeSuffix = _clientViewTypes[option];

            if( string.IsNullOrEmpty(themeSuffix) || string.IsNullOrEmpty(viewPath))
            {
                return viewPath;
            }

            var newViewPath = viewPath.Replace("Views", "Views/" + themeSuffix);
            return File.Exists(HttpContext.Current.Server.MapPath(newViewPath)) ? newViewPath : viewPath;

        }
    }
}
