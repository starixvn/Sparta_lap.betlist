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
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Configuration;

    /// <summary>
    /// Utility controller.
    /// </summary>
    public class UtilityController : BaseController
    {
        private readonly IDictionary<string, string> ChangePassPageData = new Dictionary<string, string>(){
        {"alertInputOldPwd",ViewResources.AlertInputOldPwdMsg},{"alertPasswordEmpty",ViewResources.AlertPasswordEmptyMsg},
        {"alertErrorConfirm",ViewResources.PasswordConfirmErrorMsg},{"alertPasswordRequire",ViewResources.PasswordNoteMsg},
        {"alertPasswordSucc",ViewResources.PasswordChangedMsg}
        };

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View("~/Views/Utility/AccessDenied.cshtml");
        }

        [AllowAnonymous]
        public ActionResult SSODenyAccess()
        {
            var code = Request.QueryString["Code"] != null ? Request.QueryString["Code"] : "0";
            var message = Request.QueryString["Mess"] != null ? Request.QueryString["Mess"] : string.Empty;
            if (string.IsNullOrEmpty(message))
            {
                switch (code)
                {
                    case "-1":
                        message = ViewResources.InValidParamMsg;
                        break;

                    case "-2":
                        message = ViewResources.ExpiredTimeParamMsg;

                        break;

                    case "-3":
                        message = ViewResources.DisableSSOMsg;

                        break;

                    case "-4":
                        message = ViewResources.UnAccessLoginMsg;

                        break;
                }
            }

            ViewBag.Message = message;
            return View("~/Views/Utility/SSODenyAccess.cshtml");
        }

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

        [AllowAnonymous]
        public ActionResult Error()
        {
            string errorStr = string.Empty;
            string errCode = Request.QueryString["err"];
            if (string.IsNullOrEmpty(errCode)) errCode = string.Empty;
            switch (errCode)
            {
                case "400":
                    errorStr = Fanex.Athena.Models.ViewModel.ViewResources.InvalidRequestMsg;
                    break;

                case "403":
                    errorStr = Fanex.Athena.Models.ViewModel.ViewResources.HttpError_403;
                    break;

                case "404":
                    errorStr = Fanex.Athena.Models.ViewModel.ViewResources.HttpError_404;
                    break;

                default:
                    errorStr = Fanex.Athena.Models.ViewModel.ViewResources.HttpError_Default;
                    break;
            }
            if (string.IsNullOrEmpty(errorStr))
                errorStr = Fanex.Athena.Models.ViewModel.ViewResources.HttpError_Default;

            ViewBag.Message = errorStr;
            return View("~/Views/Utility/Error.cshtml");
        }

        [AllowAnonymous]
        public ActionResult UnderMaintenance()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();

            var umTimeFrom = string.Empty;
            var umTimeTo = string.Empty;
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
                umTimeFrom = maintenanceInfo.UmTimeFrom;
                umTimeTo = maintenanceInfo.UmTimeTo;

                if (string.IsNullOrEmpty(umTimeFrom) || string.IsNullOrEmpty(umTimeTo))
                {
                    umTimeFrom = Request.QueryString["from"] != null ? Request.QueryString["from"] : string.Empty;
                    umTimeTo = Request.QueryString["to"] != null ? Request.QueryString["to"] : string.Empty;
                }
            }
            ViewBag.UmTimeFrom = umTimeFrom;
            ViewBag.UmTimeTo = umTimeTo;
            return View("~/Views/Utility/UnderMaintenance.cshtml");
        }

        [HttpGet]
        public ActionResult SelectLicensee(int? licenseeId)
        {
            var isSessionValid = SessionService.UserSession != null && SessionService.UserSession.UserId > 0;
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

                var licenseeList = licenseeService.GetByUser(SessionService.UserSession.UserId);
                ///Admin, but not assinged to handle any licensees
                if (licenseeList.Count <= 0)
                {
                    return RedirectToAction("SignIn", "Authorization");
                }

                ///Admin user handle only one licensee
                if (licenseeList.Count == 1)
                {
                    return RedirectFromLoginPage(licenseeList[0].Id);
                }

                ViewBag.CurrentUsername = SessionService.UserSession.UserName;
                ViewBag.LicenseeList = licenseeList;
            }

            return View("~/Views/Utility/SelectLicensee.cshtml");
        }

        [HttpGet]
        public ActionResult ChangePass()
        {
            ViewBag.PageData = ChangePassPageData;
            return View("~/Views/Utility/ChangePass.cshtml");
        }

        [HttpPost]
        public ActionResult ChangePass(string txtOldPwd, string txtNewPwd, string txtConfirmPwd)
        {
            if (!Util.CheckValidPassword(txtNewPwd) || string.IsNullOrEmpty(txtOldPwd))
            {
                return Json(new
                {
                    errCode = ErrorType.Unknown,
                    errMsg = ViewResources.PasswordNoteMsg
                });
            }
            var userService = new UserService();

            if (SessionService.UserSession.IsInternal)
            {
                return Json(new
                {
                    errCode = ErrorType.AccessDenied,
                    errMsg = ViewResources.AccessDeniedMsg
                });
            }
            else
            {
                IResultInfo resultInfo = userService.ChangePassword(SessionService.UserSession.UserName
                 , txtOldPwd, txtNewPwd, Request.UserHostAddress);
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