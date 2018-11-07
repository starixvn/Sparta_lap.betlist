namespace Fanex.Athena.Service.Core.Helpers
{
    using System.Web;
    using Extension.Utility.Interfaces;

    public class ClientViewHelper : IClientViewHelper
    {
        private readonly IFileSystem _file;

        public ClientViewHelper(IFileSystem file)
        {
            _file = file;
        }

        public string GetViewPath(string viewPath)
        {
            var newUIViewPath = viewPath.Replace("Views", "Views/Desktop");
            var existedNewUIPath = _file.Exists(HttpContext.Current.Server.MapPath(newUIViewPath));

            return existedNewUIPath ? newUIViewPath : viewPath;
        }
    }
}