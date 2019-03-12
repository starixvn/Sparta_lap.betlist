namespace Fanex.Athena.Result.Controller
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;
    using Fanex.Athena.Common.Utility.Attributes;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CustomAuth]
    public class BaseResultController : Controller
    {
        private const string LANGUAGEKEY = "Language";

        private string _languageCode = "en-US";

        protected string LanguageCode => _languageCode;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var languageCode = requestContext.HttpContext.Request.Headers[LANGUAGEKEY];
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
                _languageCode = languageCode;
            }
        }
    }
}