// -----------------------------------------------------------------------
// <copyright file="UtilityController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using ONEbook.UM.Services;
    using Resources;
    using Service.Core.Common.Interfaces;

    /// <summary>
    /// Utility controller.
    /// </summary>
    [AllowAnonymous]
    public class UtilityController : Controller
    {
        private readonly IAppSettings _appSettings;

        private readonly ILanguageService _languageService;

        private readonly IMaintenanceService _maintenanceService;

        public UtilityController(IAppSettings appSettings, ILanguageService languageService, IMaintenanceService maintenanceService)
        {
            _appSettings = appSettings;
            _languageService = languageService;
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public ActionResult SessionIsAlive()
        {
            return Json(HttpContext.User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AccessDenied()
        {
            return View("~/Views/Desktop/Utility/AccessDenied.cshtml");
        }

        public ActionResult OneTokenRestriction(int code)
        {
            ViewBag.Message = GetMessageRestriction(code);

            return View("~/Views/Desktop/Utility/OneTokenRestriction.cshtml");
        }

        public ActionResult LocationRestriction()
        {
            var requestHost = Request.Url.AbsoluteUri;

            if (requestHost.Contains(_appSettings.GetValue("InternalSubDomain")))
            {
                return RedirectToAction("SignIn", "Authorization");
            }

            if (requestHost.Contains(_appSettings.GetValue("LapSubDomain")))
            {
                return RedirectToAction("OneTokenRestriction", "Utility", new { code = OneAuthorizationErrorCode.TokenWasExpired });
            }

            return View("~/Views/Desktop/Utility/LocationRestriction.cshtml");
        }

        /// <summary>
        /// The error page.
        /// </summary>
        /// <returns>The error information.</returns>
        public ActionResult Error()
        {
            var errorStr = string.Empty;

            var errCode = Request.QueryString["err"];
            if (string.IsNullOrEmpty(errCode))
            {
                errCode = string.Empty;
            }

            switch (errCode)
            {
                case "400":
                    errorStr = ViewResources.InvalidRequestMsg;
                    break;

                case "403":
                    errorStr = ViewResources.HttpError_403;
                    break;

                case "404":
                    errorStr = ViewResources.HttpError_404;
                    break;

                default:
                    errorStr = ViewResources.HttpError_Default;
                    break;
            }

            return View("~/Views/Desktop/Utility/Error.cshtml", (object)errorStr);
        }

        public ActionResult UnderMaintenance(bool notReload = false)
        {
            var underMaintenanceTimeFrom = string.Empty;
            var underMaintenanceTimeTo = string.Empty;
            var maintenanceInfo = _maintenanceService.GetUnderMaintenanceTime();

            if (!maintenanceInfo.IsUnderMaintenanceTime && !notReload)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            if (maintenanceInfo.IsUnderMaintenanceTime)
            {
                underMaintenanceTimeFrom = maintenanceInfo.From;
                underMaintenanceTimeTo = maintenanceInfo.To;

                if (string.IsNullOrEmpty(underMaintenanceTimeFrom) || string.IsNullOrEmpty(underMaintenanceTimeTo))
                {
                    underMaintenanceTimeFrom = Request.QueryString["from"] ?? string.Empty;
                    underMaintenanceTimeTo = Request.QueryString["to"] ?? string.Empty;
                }
            }

            ViewBag.UmTimeFrom = underMaintenanceTimeFrom;
            ViewBag.UmTimeTo = underMaintenanceTimeTo;
            return View("~/Views/Desktop/Utility/UnderMaintenance.cshtml");
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var currentLanguage = _languageService.GetCurrentLanguage();
            _languageService.SetPageCulture(currentLanguage);
        }

        private static string GetMessageRestriction(int code)
        {
            switch (code)
            {
                case OneAuthorizationErrorCode.TokenWasExpired:
                    return ViewResources.TokenWasExpired;

                case OneAuthorizationErrorCode.LicenseeIsNotExisted:
                    return ViewResources.LicenseeIsNotExisted;

                case OneAuthorizationErrorCode.TokenIsInValid:
                    return ViewResources.TokenIsInValid;

                case OneAuthorizationErrorCode.LocationRestrictionMsg:
                    return string.Format(ViewResources.LocationRestrictionMsg, ViewResources.Administrator);

                case OneAuthorizationErrorCode.UserNotHaveCurrency:
                    return ViewResources.UserNotHaveCurrency;

                case OneAuthorizationErrorCode.YouAreNotAllowToAccessThisPage:
                    return ViewResources.YouAreNotAllowToAccessThisPage;

                case OneAuthorizationErrorCode.NotFoundUser:
                    return ViewResources.UserNotExist;

                case OneAuthorizationErrorCode.InvalidSubsidiaryNameMsg:
                    return ViewResources.InvalidSubsidiaryNameMsg;

                default:
                    return ViewResources.OneTokenRestrictionDefaultMsg;
            }
        }
    }
}