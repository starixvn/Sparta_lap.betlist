// -----------------------------------------------------------------------
// <copyright file="UtilityController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Extension.Utility;
    using System.Web.Security;

    /// <summary>
    /// Utility controller.
    /// </summary>
    public class UtilityController : BaseController
    {
        public ActionResult AccessDenied()
        {
            return View("~/Views/Utility/AccessDenied.cshtml");
        }

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

        public ActionResult LocationRestriction()
        {
            var requestHost = Request.Url.AbsoluteUri;
            var isInternalDomain = requestHost.Contains(AppSettings.GetValue("internaldomain"));

            if (isInternalDomain)
            {
                return RedirectToAction("SignIn", "Authorization");
            }

            return View("~Views/Utility/LocationRestriction.cshtml");
        }

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
            return View("~Views/Utility/Error.cshtml");
        }

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
            return View("~Views/Utility/UnderMaintenance.cshtml");
        }
    }
}