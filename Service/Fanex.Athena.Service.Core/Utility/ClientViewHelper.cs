namespace Fanex.Athena.Service.Core.Helpers
{
    using System.IO;
    using System.Web;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Extension.Utility.Interfaces;
    using Extension.Utility;

    public class ClientViewHelper : IClientViewHelper
    {
        private readonly IFileSystem _file;
        public ClientViewHelper() : this(null)
        {

        }
        public ClientViewHelper(IFileSystem file)
        {
            _file = file ?? new FileSystem();
        }
        public ViewByOption GetViewPath(string viewPath, ClientViewType viewtype)
        {
            const string linkToNewUI = "~/Views/Shared/FunctionNotAvailableOldUI.cshtml";

            var newUIViewPath = viewPath.Replace("Views", "Views/Desktop");
            var existedNewUIPath = _file.Exists(HttpContext.Current.Server.MapPath(newUIViewPath));

            if (viewtype == ClientViewType.Desktop)
            {
                return new ViewByOption
                {
                    ViewPath = existedNewUIPath ? newUIViewPath : viewPath,
                    IsNavigationView = false
                };
            }

            var existedOldUIPath = _file.Exists(HttpContext.Current.Server.MapPath(viewPath));

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