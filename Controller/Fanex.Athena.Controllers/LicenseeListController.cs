namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using DataAccess.Core.Users;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [AuthorizeRestriction(AppMode.Self)]
    public class LicenseeListController : BaseController
    {
        private readonly ILicenseeDataAccess _licenseeDataAccess;

        public LicenseeListController()
        {
            _licenseeDataAccess = new LicenseeDataAccess();
        }

        public LicenseeListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            ILicenseeDataAccess licenseeDataAccess)
            : base(languageService, sessionService, appSetting, fileSystem)
        {
            _licenseeDataAccess = licenseeDataAccess;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? licenseeId)
        {
            var userSession = SessionService.UserSession;
            if (userSession == null || userSession.UserId <= 0)
            {
                return RedirectToAction("SignIn", "Authorization");
            }

            if (licenseeId.HasValue && licenseeId.Value > 0)
            {
                return RedirectToHome(licenseeId.Value);
            }
            else
            {
                var licenseeList = _licenseeDataAccess.GetByUser(userSession.UserId);

                if (licenseeList.Count <= 0)
                {
                    return RedirectToAction("SignIn", "Authorization");
                }

                if (licenseeList.Count == 1)
                {
                    return RedirectToHome(licenseeList[0].Id);
                }

                ViewBag.CurrentUsername = userSession.UserName;
                ViewBag.LicenseeList = licenseeList;
            }

            return View("~/Views/LicenseeList/Index.cshtml");
        }

        private ActionResult RedirectToHome(int licenseeId)
        {
            SessionService.UpdateLiceseeSession(licenseeId);

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}