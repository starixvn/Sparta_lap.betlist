// -----------------------------------------------------------------------
// <copyright file="LiveReportController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel.LiveReports;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index(bool isViewedHistory = false)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var balance = BalanceUpService.BalanceUpInfo;
            var metaModel = new LiveReportMetaModel
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

            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var model = new LiveReportViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedNumberGame = productAccess.IsAllowNG,
                BalanceUpDate = balance != null ? balance.BalanceUpDay : DateTime.Now,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess
            };

            return View("~/Views/Report/LiveReport/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(LiveReportMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var isExportedExcel = metaModel.IsExportExcel || Request.Form["exporttoexcel.x"] != null;
            var balance = BalanceUpService.BalanceUpInfo;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var productAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language);
            var liveReportService = new LiveReportService();
            var model = new LiveReportViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedNumberGame = productAccess.IsAllowNG,
                BalanceUpDate = balance != null ? balance.BalanceUpDay : DateTime.Now,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                MainResult = liveReportService.GetLiveReportSummary(metaModel)
            };

            return isExportedExcel
                       ? Excel(model)
                       : View("~/Views/Report/LiveReport/Main.cshtml", model);
        }

        [HttpGet]
        public ActionResult MatchList(LiveReportMatchListMetalModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
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

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public ActionResult Detail(LiveReportMetaModel metaModel, string productCode)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var balanceUpDay = this.BalanceUpService.BalanceUpInfo.BalanceUpDay;

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
                    IsShowedDetailLink = metaModel.FromDate >= balanceUpDay,
                    Param = metaModel,
                    ProductAccess = productAccess
                });
        }

        private ActionResult Excel(LiveReportViewModel viewModel)
        {
            var templatePath = Server.MapPath("~/App_Data/Template/Reports/LiveReport.xls");
            const string FileName = "LiveReport";
            var productCode = Request.Form["productCode"];
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
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
                convertingOption,
                showingOption,
                viewModel.MainResult,
                liveReportDetail);
            liveReportExcel.Export();

            return new ExcelResult(liveReportExcel.Workbook, FileName);
        }
    }
}