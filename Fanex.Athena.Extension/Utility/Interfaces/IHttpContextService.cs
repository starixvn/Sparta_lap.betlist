namespace Fanex.Athena.Extension.Utility.Interfaces
{
    using System.Web;
    using System.Web.SessionState;

    public interface IHttpContextService
    {
        HttpRequest Request { get; }

        HttpSessionState Session { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        void Redirect(string url);

        string MapPath(string path);
    }
}