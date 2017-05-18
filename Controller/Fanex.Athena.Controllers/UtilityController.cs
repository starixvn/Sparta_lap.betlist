// -----------------------------------------------------------------------
// <copyright file="UtilityController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using Resources;
    using Service.Core.Common;
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

        public UtilityController() : base()
        {
            _appSettings = new AppSettings();
            _languageService = new LanguageService();
            _maintenanceService = new MaintenanceService();
        }

        internal UtilityController(IAppSettings appSettings, ILanguageService languageService, IMaintenanceService maintenanceService)
        {
            _appSettings = appSettings ?? new AppSettings();
            _languageService = languageService ?? new LanguageService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
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

        public ActionResult SSODenyAccess()
        {
            int code;
            var message = string.Empty;

            if (!int.TryParse(Request.QueryString["Code"] != null ? Request.QueryString["Code"] : "0", out code))
            {
                code = -1;
            }

            switch (code)
            {
                case 1:
                    message = ViewResources.SSOLinkIncorrectMsg;
                    break;

                case 2:
                    message = ViewResources.DisableSSOMsg;
                    break;

                case 100:

                    message = ViewResources.SSOLinkExpiredMsg;
                    break;

                case 101:
                    message = ViewResources.WrongUserNamePassword;
                    break;

                case 102:
                    message = ViewResources.UserNotActive;
                    break;

                case 103:
                    message = ViewResources.UserTooManyLicensees;
                    break;

                case 104:
                    message = ViewResources.UserNoLicensee;
                    break;

                default:
                    message = ViewResources.InvalidRequestMsg;
                    break;
            }

            ViewBag.Message = message;
            return View("~/Views/Desktop/Utility/SSODenyAccess.cshtml");
        }

        public ActionResult LocationRestriction()
        {
            var requestHost = Request.Url.AbsoluteUri;
            var isInternalDomain = requestHost.Contains(_appSettings.GetValue("internaldomain"));

            return isInternalDomain ? (ActionResult)RedirectToAction("SignIn", "Authorization") : View("~/Views/Desktop/Utility/LocationRestriction.cshtml");
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

            ViewBag.Message = errorStr;
            return View("~/Views/Desktop/Utility/Error.cshtml");
        }

        public ActionResult UnderMaintenance(bool notReload = false)
        {
            var underMaintenanceTimeFrom = string.Empty;
            var underMaintenanceTimeTo = string.Empty;
            var maintenanceInfo = _maintenanceService.GetUnderMaintenanceTime();

            if (maintenanceInfo.IsUnderMaintenanceTime)
            {
                underMaintenanceTimeFrom = maintenanceInfo.From;
                underMaintenanceTimeTo = maintenanceInfo.To;

                if (string.IsNullOrEmpty(underMaintenanceTimeFrom) || string.IsNullOrEmpty(underMaintenanceTimeTo))
                {
                    underMaintenanceTimeFrom = Request.QueryString["from"] != null ? Request.QueryString["from"] : string.Empty;
                    underMaintenanceTimeTo = Request.QueryString["to"] != null ? Request.QueryString["to"] : string.Empty;
                }
            }
            else if (!notReload)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            ViewBag.UmTimeFrom = underMaintenanceTimeFrom;
            ViewBag.UmTimeTo = underMaintenanceTimeTo;
            return View("~/Views/Utility/UnderMaintenance.cshtml");
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var currentLanguage = _languageService.GetCurrentLanguage();
            _languageService.SetPageCulture(currentLanguage);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            var option = (ClientViewType)int.Parse(ConfigurationManager.AppSettings["ClientViewType"]);

            var viewByOption = ClientViewHelper.GetViewPath(viewName, option);
            return base.View(viewByOption, masterName, model);
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

                case OneAuthorizationErrorCode.LicenseeNotOfferedCurrency:
                    return ViewResources.LicenseeNotOfferedCurrency;

                case OneAuthorizationErrorCode.YouAreNotAllowToAccessThisPage:
                    return ViewResources.YouAreNotAllowToAccessThisPage;

                case OneAuthorizationErrorCode.NotFoundUser:
                    return ViewResources.UserNotExist;

                default:
                    return ViewResources.OneTokenRestrictionDefaultMsg;
            }
        }
    }
}