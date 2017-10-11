namespace Fanex.Athena.Extension.Utility
{
    using System.Web;
    using System.Web.SessionState;
    using Interfaces;

    public class HttpContextService : IHttpContextService
    {
        private readonly HttpContext _context;

        public HttpContextService()
        {
            _context = HttpContext.Current;
        }

        public HttpRequest Request
        {
            get
            {
                return _context.Request;
            }
        }

        public HttpSessionState Session
        {
            get
            {
                return _context.Session;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3994:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public virtual void Redirect(string url)
        {
            _context.Response.Redirect(url);
        }

        public virtual string MapPath(string path)
        {
            return _context.Server.MapPath(path);
        }
    }
}