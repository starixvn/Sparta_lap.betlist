namespace Fanex.Athena.Extension.Utility.Interfaces
{
    using System.Web;
    using System.Web.SessionState;

    public interface IHttpContextService
    {
        HttpRequest Request { get; }

        HttpSessionState Session { get; }

        void Redirect(string url);

        string MapPath(string path);
    }
}