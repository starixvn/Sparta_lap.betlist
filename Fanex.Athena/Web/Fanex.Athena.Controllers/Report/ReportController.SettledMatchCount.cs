// -----------------------------------------------------------------------
// <copyright file="ReportController.SettledMatchCount.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Implement.Report;
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
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var isShowAll = AppSettings.IsTesting;
            var producAccess = ProductService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var settledMatchCountService = new SettledMatchCountService();

            var model = settledMatchCountService.GetDataList(isShowAll, licenseeId, producAccess);

            return View("~/Views/Report/SettledMatchCount/Main.cshtml", model);
        }
    }
}