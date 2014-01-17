// -----------------------------------------------------------------------
// <copyright file="IntegrationController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Integration
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Logging;
    using LASAPI = LicenseeAgent.API;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IntegrationController : BaseController
    {
        /// <summary>
        /// Create  bridged link between LRF and LAS system.
        /// </summary>
        /// <returns>The JSON result.</returns>
        [AjaxRequest]
        [AjaxFunctionAuthorization]
        public ActionResult LicenseeAgent()
        {
            var errorMsg = "Found Error when calling [LicenseeAgent.API] to generate  bridged link between LRF and LAS system\n";
            var bridgedLink = string.Empty;
            var useSesstion = SessionService.UserSession;
            var licenseeSesstion = SessionService.LicenseeSession;
            try
            {
                bridgedLink = LASAPI.LRF.Integration.GenerateLink(useSesstion.UserId, useSesstion.OnUserId, licenseeSesstion.LicenseeId, useSesstion.Language);
            }
            catch (Exception exception)
            {
                LogService.Error(errorMsg + exception.Message, exception);
            }

            return Json(new
            {
                IsOk = !string.IsNullOrEmpty(bridgedLink),
                Url = bridgedLink
            }, JsonRequestBehavior.AllowGet);
        }
    }
}