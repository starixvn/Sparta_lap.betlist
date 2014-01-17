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
            var isShowAll = AppSettings.IsTesting;
            var producAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            var model = _settledMatchCountService.GetDataList(isShowAll, licenseeId, producAccess);

            return View("~/Views/Report/SettledMatchCount/Main.cshtml", model);
        }
    }
}