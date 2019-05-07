// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Service.Core.Users.Interfaces;

    [CustomAuthorize]
    [XFrameOption]
    public class CultureBaseController : Controller
    {
        protected const string DefaultLanguage = "en-US";

        private readonly ILanguageService _langService;

        private readonly ISessionService _sessionService;

        private readonly IAppSettings _appSettings;

        protected CultureBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSettings)
        {
            _langService = languageService;
            _sessionService = sessionService;
            _appSettings = appSettings;
        }

        public ILanguageService LangService
        {
            get { return _langService; }
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        public IAppSettings AppSettings
        {
            get { return _appSettings; }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
        }

        protected bool IsFakeLicensee(int licenseeId)
        {
            return AppSettings.FakeLicenseeList.Contains(licenseeId);
        }
    }
}