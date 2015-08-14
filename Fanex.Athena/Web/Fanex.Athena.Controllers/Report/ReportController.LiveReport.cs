// -----------------------------------------------------------------------
// <copyright file="ReportController.LiveReport.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Report.LiveReports;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report.LiveReports;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// The live report controller.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// The live report.
        /// </summary>
        /// <param name="isViewedHistory">True if you want to view history report.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult LiveReport(bool isViewedHistory = false)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var balance = _balanceUpService.BalanceUpInfo;
            var metaModel = new LiveReportMetalModel
                {
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    LicenseeId = licenseeSession.LicenseeId,
                    UserId = userSession.UserId,
                    IsViewedHistory = isViewedHistory,
                    CurrencyId = 0,
                    LiveIndicator = 1
                };

            SetConvetedUSDCurrencyName(metaModel);

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var model = new LiveReportViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = GetUserCurrencies(Licensees.KeenOcean),
                    IsAllowedNumberGame = productAccess.IsAllowNG,
                    BalanceUpDate = balance != null ? balance.BalanceUpDay : DateTime.Now,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    ProductAccess = productAccess
                };

            return View("~/Views/Report/LiveReport/Main.cshtml", model);
        }

        /// <summary>
        /// The live report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult LiveReport(LiveReportMetalModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var isExportedExcel = Request.Form["exporttoexcel.x"] != null;
            var balance = _balanceUpService.BalanceUpInfo;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            var productAccess = _productService.GetProductAccess(metaModel.LicenseeId, userSession.Language);
            var liveReportService = new LiveReportService();
            var model = new LiveReportViewModel
                {
                    Param = metaModel,
                    CurrencyList = GetUserCurrencies(Licensees.KeenOcean),
                    IsAllowedNumberGame = productAccess.IsAllowNG,
                    BalanceUpDate = balance != null ? balance.BalanceUpDay : DateTime.Now,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    ProductAccess = productAccess,
                    MainResult = liveReportService.GetLiveReportSummary(metaModel)
                };

            return isExportedExcel
                       ? LiveReportExcel(model)
                       : View("~/Views/Report/LiveReport/Main.cshtml", model);
        }

        /// <summary>
        /// The match list report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportMatchList(LiveReportMatchListMetalModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var liveReportService = new LiveReportService();
            var dataList = liveReportService.GetLiveReportWinLossByMatch(metaModel);

            var model = new LiveReportMatchListViewModel
                {
                    MainResult = dataList,
                    Param = metaModel,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    ProductAccess = productAccess,
                    CurrencyName = metaModel.CurrencyName
                };

            return View("~/Views/Report/LiveReport/MatchList/Content.cshtml", model);
        }

        #region Export Excel

        /// <summary>
        /// The live report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        private ActionResult LiveReportExcel(LiveReportViewModel viewModel)
        {
            var templatePath = Server.MapPath("~/App_Data/Template/Reports/LiveReport.xls");
            const string FileName = "LiveReport";
            var productCode = Request.Form["productCode"];
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var showingOption = new ShowingOption
                {
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                    BAVisible = productAccess.IsAllowBA,
                    RBVisible = productAccess.IsAllowRB
                };
            var convertingOption = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            IList<LiveReport> dataByCurrency = null;
            IList<LiveReport> dataBySport = null;
            IList<LiveReportByBetType> dataByBetType = null;

            if (!string.IsNullOrEmpty(productCode))
            {
                var productList = productAccess.ProductList.Where(i => i.ProductCode == productCode || productCode == "ALL").ToList();
                var liveReportService = new LiveReportService();
                liveReportService.GetLiveReportSumaryDetail(viewModel.Param, productCode, productList, out dataBySport, out dataByCurrency, out dataByBetType);
            }

            var liveReportDetail = new LiveReportDetailViewModel
                {
                    BySports = dataBySport,
                    ByCurrencies = dataByCurrency,
                    ByBetType = dataByBetType,
                    Param = viewModel.Param,
                };
            var liveReportExcel = new LiveReportExcel(
                templatePath,
                FileName,
                convertingOption,
                showingOption,
                viewModel.MainResult,
                liveReportDetail);
            liveReportExcel.Export();

            return new ExcelResult(liveReportExcel.Workbook, FileName);
        }

        #endregion Export Excel
    }
}