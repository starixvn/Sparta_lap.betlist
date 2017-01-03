namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DataDomain;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [AuthorizeRestriction(AppMode.Self)]
    public class LicenseeListController : BaseController
    {
        private readonly ILicenseeService _licenseeService;

        public LicenseeListController()
        {
            _licenseeService = new LicenseeService();
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
                var licenseeList = _licenseeService.GetByUser(userSession.UserId);
                //// Admin, but not assinged to handle any licensees
                if (licenseeList.Count <= 0)
                {
                    return RedirectToAction("SignIn", "Authorization");
                }

                //// Admin user handle only one licensee
                if (licenseeList.Count == 1)
                {
                    return RedirectToHome(licenseeList[0].Id);
                }

                ViewBag.CurrentUsername = userSession.UserName;
                ViewBag.LicenseeList = licenseeList;
            }

            return View("~/Views/Utility/SelectLicensee.cshtml");
        }

        private ActionResult RedirectToHome(int licenseeId)
        {
            SessionService.UpdateLiceseeSession(licenseeId);

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}