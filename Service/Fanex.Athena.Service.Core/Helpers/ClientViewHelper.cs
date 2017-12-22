namespace Fanex.Athena.Service.Core.Helpers
{
    using System.IO;
    using System.Web;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.DTO.Common;

    public static class ClientViewHelper
    {
        public static ViewByOption GetViewPath(string viewPath, ClientViewType viewtype)
        {
            const string linkToNewUI = "~/Views/Shared/FunctionNotAvailableOldUI.cshtml";

            var newUIViewPath = viewPath.Replace("Views", "Views/Desktop");
            var existedOldUIPath = File.Exists(HttpContext.Current.Server.MapPath(viewPath));
            var existedNewUIPath = File.Exists(HttpContext.Current.Server.MapPath(newUIViewPath));

            if (viewtype == ClientViewType.Desktop)
            {
                return new ViewByOption
                {
                    ViewPath = existedNewUIPath ? newUIViewPath : viewPath,
                    IsNavigationView = false
                };
            }

            if (!existedOldUIPath && existedNewUIPath)
            {
                return new ViewByOption
                {
                    ViewPath = linkToNewUI,
                    IsNavigationView = true
                };
            }

            return new ViewByOption
            {

                ViewPath = viewPath,
                IsNavigationView = false
            };

        }
    }
}