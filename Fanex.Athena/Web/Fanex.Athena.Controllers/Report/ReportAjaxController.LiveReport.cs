// -----------------------------------------------------------------------
// <copyright file="ReportAjaxController.LiveReport.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report.LiveReports;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report.LiveReports;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Live report ajax controller.
    /// </summary>
    public partial class ReportController
    {
        #region LiveReport

        /// <summary>
        /// The live report detail.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="productCode">The product code.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult LiveReportDetail(LiveReportMetalModel metaModel, string productCode)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var balanceUpDay = this._balanceUpService.BalanceUpInfo.BalanceUpDay;

            IList<LiveReport> dataByCurrency;
            IList<LiveReport> dataBySport;
            IList<LiveReportByBetType> dataByBetType;

            var productList = productAccess.ProductList.Where(i => i.ProductCode == productCode || productCode == "ALL").ToList();
            var liveReportService = new LiveReportService();
            liveReportService.GetLiveReportSumaryDetail(metaModel, productCode, productList, out dataBySport, out dataByCurrency, out dataByBetType);

            return View(
                "~/Views/Report/LiveReport/AjaxTemplate/LiveReportDetail.cshtml",
                new LiveReportDetailViewModel
                {
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    BySports = dataBySport,
                    ByCurrencies = dataByCurrency,
                    ByBetType = dataByBetType,
                    IsShowedDetailLink = metaModel.FromDate >= balanceUpDay && !productCode.Equals(Product.KN, System.StringComparison.OrdinalIgnoreCase),
                    Param = metaModel,
                    ProductAccess = productAccess
                });
        }

        #endregion LiveReport
    }
}