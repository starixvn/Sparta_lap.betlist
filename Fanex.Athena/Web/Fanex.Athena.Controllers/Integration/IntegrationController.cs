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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public ActionResult LicenseeAgent()
        {
            var errorMsg = "Found Error when calling [LicenseeAgent.API] to generate  bridged link between LRF and LAS system\n";
            var bridgedLink = string.Empty;
            var useSesstion = SessionService.UserSession;
            var licenseeSesstion = SessionService.LicenseeSession;
            try
            {
                var integration = new LicenseeAgent.API.LRF.Integration();
                bridgedLink = integration.GenerateLink(useSesstion.UserId, useSesstion.OnUserId, licenseeSesstion.LicenseeId, useSesstion.IsInternal, useSesstion.Language);
            }
            catch (Exception exception)
            {
                LogService.Error(errorMsg + exception.Message, exception);
            }

            return Json(
                new
                {
                    IsOk = !string.IsNullOrEmpty(bridgedLink),
                    Url = bridgedLink
                },
            JsonRequestBehavior.AllowGet);
        }
    }
}