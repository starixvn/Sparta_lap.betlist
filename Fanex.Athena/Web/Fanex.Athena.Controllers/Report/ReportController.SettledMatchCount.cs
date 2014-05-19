// -----------------------------------------------------------------------
// <copyright file="ReportController.SettledMatchCount.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        #region Const

        #endregion Const

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult SettledMatchCount()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var isShowAll = AppSettings.IsTesting;
            var producAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var licenseeId = licenseeSession.LicenseeId;

            var model = _settledMatchCountService.GetDataList(isShowAll, licenseeId, producAccess);

            return View("~/Views/Report/SettledMatchCount/Main.cshtml", model);
        }
    }
}