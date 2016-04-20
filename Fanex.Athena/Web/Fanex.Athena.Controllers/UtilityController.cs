// -----------------------------------------------------------------------
// <copyright file="UtilityController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Resources;

    /// <summary>
    /// Utility controller.
    /// </summary>
    public class UtilityController : BaseController
    {
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View("~/Views/Utility/AccessDenied.cshtml");
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
            var isInternalDomain = requestHost.Contains(AppSettings.GetValue("internaldomain"));
            if (isInternalDomain)
            {
                return RedirectToAction("SignIn", "Authorization");
            }
            else
            {
                return View("~/Views/Utility/LocationRestriction.cshtml");
            }
        }

        /// <summary>
        /// The error page.
        /// </summary>
        /// <returns>The error information.</returns>
        [AllowAnonymous]
        public ActionResult Error()
        {
            string errorStr = string.Empty;
            string errCode = Request.QueryString["err"];
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

        /// <summary>
        /// The under maintenance page.
        /// </summary>
        /// <returns>The maintenance information.</returns>
        [AllowAnonymous]
        public ActionResult UnderMaintenance()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();

            var underMaintenanceTimeFrom = string.Empty;
            var underMaintenanceTimeTo = string.Empty;
            var maintenanceInfo = maintenanceService.MaintenanceInfo;
            var acceptIp = AppSettings.GetValue("AcceptIp");
            var isDeploying = acceptIp != "*" && Request.UserHostAddress != acceptIp;

            if (!isDeploying && !maintenanceInfo.IsDownTime)
            {
                Response.Redirect(FormsAuthentication.LoginUrl);
                Response.End();
            }
            else
            {
                underMaintenanceTimeFrom = maintenanceInfo.UmTimeFrom;
                underMaintenanceTimeTo = maintenanceInfo.UmTimeTo;

                if (string.IsNullOrEmpty(underMaintenanceTimeFrom) || string.IsNullOrEmpty(underMaintenanceTimeTo))
                {
                    underMaintenanceTimeFrom = Request.QueryString["from"] != null ? Request.QueryString["from"] : string.Empty;
                    underMaintenanceTimeTo = Request.QueryString["to"] != null ? Request.QueryString["to"] : string.Empty;
                }
            }

            ViewBag.UmTimeFrom = underMaintenanceTimeFrom;
            ViewBag.UmTimeTo = underMaintenanceTimeTo;
            return View("~/Views/Utility/UnderMaintenance.cshtml");
        }

        /// <summary>
        /// The licensee list page.
        /// </summary>
        /// <param name="licenseeId">The licensee ID.</param>
        /// <returns>The licensee list.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SelectLicensee(int? licenseeId)
        {
            var userSession = SessionService.UserSession;
            var isSessionValid = userSession != null && userSession.UserId > 0;
            if (!isSessionValid)
            {
                return RedirectToAction("SignIn", "Authorization");
            }

            if (licenseeId.HasValue && licenseeId.Value > 0)
            {
                return RedirectFromLoginPage(licenseeId.Value);
            }
            else
            {
                var licenseeService = new LicenseeService();

                var licenseeList = licenseeService.GetByUser(userSession.UserId);
                //// Admin, but not assinged to handle any licensees
                if (licenseeList.Count <= 0)
                {
                    return RedirectToAction("SignIn", "Authorization");
                }

                //// Admin user handle only one licensee
                if (licenseeList.Count == 1)
                {
                    return RedirectFromLoginPage(licenseeList[0].Id);
                }

                ViewBag.CurrentUsername = userSession.UserName;
                ViewBag.LicenseeList = licenseeList;
            }

            return View("~/Views/Utility/SelectLicensee.cshtml");
        }

        /// <summary>
        /// The changing password page.
        /// </summary>
        /// <returns>The changing password form.</returns>
        [HttpGet]
        public ActionResult ChangePass()
        {
            ViewBag.PageData = new Dictionary<string, string>()
            {
            { "alertInputOldPwd", ViewResources.AlertInputOldPwdMsg },
            { "alertPasswordEmpty", ViewResources.AlertPasswordEmptyMsg },
            { "alertErrorConfirm", ViewResources.PasswordConfirmErrorMsg },
            { "alertPasswordRequire", ViewResources.PasswordNoteMsg },
            { "alertPasswordSucc", ViewResources.PasswordChangedMsg }
            };
            return View("~/Views/Utility/ChangePass.cshtml");
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="txtOldPwd">The old password.</param>
        /// <param name="txtNewPwd">The new password.</param>
        /// <returns>The result of changing password.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(string txtOldPwd, string txtNewPwd)
        {
            var userSession = SessionService.UserSession;
            if (!Util.CheckValidPassword(txtNewPwd) || string.IsNullOrEmpty(txtOldPwd))
            {
                return Json(new
                {
                    errCode = ErrorType.Unknown,
                    errMsg = ViewResources.PasswordNoteMsg
                });
            }

            var userService = new UserService();

            if (userSession.IsInternal)
            {
                return Json(new
                {
                    errCode = ErrorType.AccessDenied,
                    errMsg = ViewResources.AccessDeniedMsg
                });
            }
            else
            {
                IResultInfo resultInfo = userService.ChangePassword(
                    userSession.UserName,
                    txtOldPwd,
                    txtNewPwd,
                    Request.UserHostAddress);
                return Json(new
                {
                    errCode = resultInfo.Code,
                    errMsg = resultInfo.Message
                });
            }
        }

        private ActionResult RedirectFromLoginPage(int licenseeId)
        {
            SessionService.UpdateLiceseeSession(licenseeId);

            return RedirectToAction("Index", "Home");
        }
    }
}