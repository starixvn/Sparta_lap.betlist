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
    using Model.Core.DataDomain;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using Resources;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    /// <summary>
    /// Utility controller.
    /// </summary>
    public class UtilityController : Controller
    {
        private readonly ILanguageService _languageService;

        private readonly IMaintenanceService _maintenanceService;

        public UtilityController() : base()
        {
            _languageService = new LanguageService();
            _maintenanceService = new MaintenanceService();
        }

        internal UtilityController(ILanguageService languageService, IMaintenanceService maintenanceService)
        {
            _languageService = languageService ?? new LanguageService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View("~/Views/Utility/AccessDenied.cshtml");
        }

        [AllowAnonymous]
        public ActionResult OneTokenRestriction(int code)
        {
            ViewBag.Message = GetMessageRestriction(code);

            return View("~/Views/Utility/OneTokenRestriction.cshtml");
        }

        /// <summary>
        /// The denied page.
        /// </summary>
        /// <returns>The denied message.</returns>
        [AllowAnonymous]
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
            return View("~/Views/Utility/SSODenyAccess.cshtml");
        }

        /// <summary>
        /// The location restriction page.
        /// </summary>
        /// <returns>The restriction information.</returns>
        [AllowAnonymous]
        public ActionResult LocationRestriction()
        {
            var requestHost = Request.Url.AbsoluteUri;
            var isInternalDomain = requestHost.Contains(ConfigurationManager.AppSettings["internaldomain"]);

            return isInternalDomain ? (ActionResult)RedirectToAction("SignIn", "Authorization") : View("~/Views/Utility/LocationRestriction.cshtml");
        }

        /// <summary>
        /// The error page.
        /// </summary>
        /// <returns>The error information.</returns>
        [AllowAnonymous]
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

            if (string.IsNullOrEmpty(errorStr))
            {
                errorStr = ViewResources.HttpError_Default;
            }

            ViewBag.Message = errorStr;
            return View("~/Views/Utility/Error.cshtml");
        }

        [AllowAnonymous]
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