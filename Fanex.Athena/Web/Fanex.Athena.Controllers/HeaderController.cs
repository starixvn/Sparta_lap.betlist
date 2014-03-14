// -----------------------------------------------------------------------
// <copyright file="HeaderController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HeaderController : Controller
    {
        private readonly ILanguageService _langService;
        private readonly ISessionService _sessionService;
        private readonly ILicenseeService _licenseeService;

        public HeaderController()
        {
            _sessionService = new SessionService();
            _licenseeService = new LicenseeService();
            _langService = new LanguageService();
        }

        /// <summary>
        /// Header page.
        /// </summary>
        /// <returns>The header page.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var userSession = _sessionService.UserSession;
            var viewModel = new HeaderPageViewModel
            {
                Language = userSession.Language,
                OnUserId = userSession.OnUserId,
                LicenseeList = _licenseeService.GetByUser(userSession.UserId),
                IsInternalLogin = userSession.IsInternal,
                IsDisabledRecheckIn = AppSettings.LocalTest,
                SupportedLanguageList = Util.GetSupportedLanguage(),
                UserName = userSession.UserName,
                CurrentLicenseeId = _sessionService.LicenseeSession.LicenseeId
            };
            return View("~/Views/Header/Index.cshtml", viewModel);
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var currentLanguage = _langService.GetCurrentLanguage();
            _langService.SetPageCulture(currentLanguage);
            base.Initialize(requestContext);
        }

        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            var currentLanguage = _langService.GetCurrentLanguage();
            _langService.SetPageCulture(currentLanguage);
            base.Execute(requestContext);
        }
    }
}