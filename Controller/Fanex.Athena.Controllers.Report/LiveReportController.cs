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
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel.LiveReports;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Report.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportController : ReportBaseController
    {
        private const string ProductCodeAll = "ALL";

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;
        private readonly ILiveReportService _liveReportService;

        public LiveReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ILiveReportService liveReportService)
            : base(languageService, sessionService, appSetting, productService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _liveReportService = liveReportService;
        }

        [HttpGet]
        public ActionResult Index(bool isViewedHistory = false)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
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

            var productAccess = GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var model = new LiveReportViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedNumberGame = productAccess.IsAllowNG,
                BalanceUpDate = GetBalanceUpDay(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                IsFakeLicensee = IsFakeLicensee()
            };

            return View("~/Views/Report/LiveReport/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(LiveReportMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var productAccess = GetProductAccess(metaModel.LicenseeId, userSession.Language);
            metaModel.ProductCodeList = string.Join(",", productAccess.ProductList.Select(x => x.ProductCode));
            var model = new LiveReportViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedNumberGame = productAccess.IsAllowNG,
                BalanceUpDate = GetBalanceUpDay(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                MainResult = _liveReportService.GetLiveReportSummary(metaModel),
                IsFakeLicensee = IsFakeLicensee()
            };

            return metaModel.IsExportExcel
                       ? Excel(model)
                       : View("~/Views/Report/LiveReport/Main.cshtml", model);
        }

        [HttpGet]
        public ActionResult MatchList(LiveReportMatchListMetalModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var productAccess = GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var dataList = _liveReportService.GetLiveReportWinLossByMatch(metaModel);

            var model = new LiveReportMatchListViewModel
            {
                MainResult = dataList,
                Param = metaModel,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                CurrencyName = metaModel.CurrencyName,
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

            var productAccess = GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var balanceUpDay = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay;

            var resultModel = GetDetailResult(metaModel, productAccess, productCode);

            return View(
                "~/Views/Report/LiveReport/AjaxTemplate/LiveReportDetail.cshtml",
                new LiveReportDetailViewModel
                {
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    BySports = resultModel.DataBySport,
                    ByCurrencies = resultModel.DataByCurrency,
                    ByBetType = resultModel.DataByBetType,
                    IsShowedDetailLink = metaModel.FromDate >= balanceUpDay,
                    Param = metaModel,
                    ProductAccess = productAccess
                });
        }

        private LiveReportDetailResultModel GetDetailResult(LiveReportMetaModel metaModel, ProductAccess productAccess, string productCode)
        {
            IEnumerable<Product> productList;
            if (productCode == ProductCodeAll)
            {
                productList = productAccess.ProductList.ToList();
                metaModel.ProductCodeList = string.Join(",", productAccess.ProductList.Select(x => x.ProductCode));
            }
            else
            {
                productList = productAccess.ProductList.Where(i => i.ProductCode == productCode).ToList();
                metaModel.ProductCodeList = productCode;
            }

            return _liveReportService.GetLiveReportSumaryDetail(metaModel, productList);
        }

        private ActionResult Excel(LiveReportViewModel viewModel)
        {
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/LiveReport.xls");
            const string FileName = "LiveReport";
            var productCode = Request.Form["productCode"];
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                BAVisible = productAccess.IsAllowBA
            };
            var convertingOption = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var resultModel = string.IsNullOrEmpty(productCode) ?
                new LiveReportDetailResultModel() :
                GetDetailResult(viewModel.Param, productAccess, productCode);

            var liveReportDetail = new LiveReportDetailViewModel
            {
                BySports = resultModel.DataBySport,
                ByCurrencies = resultModel.DataByCurrency,
                ByBetType = resultModel.DataByBetType,
                Param = viewModel.Param,
            };
            var excel = CreateExcel(templatePath, viewModel, showingOption, convertingOption, liveReportDetail);
            excel.Export();

            return new ExcelResult(excel.Workbook, FileName);
        }

        protected internal virtual LiveReportExcel CreateExcel(
            string templatePath,
            LiveReportViewModel viewModel,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            LiveReportDetailViewModel liveReportDetail)
        {
            return new LiveReportExcel(templatePath, convertingOption, showingOption, viewModel.MainResult, liveReportDetail);
        }

        protected ProductAccess GetProductAccess(int licenseeId, string language) => ProductService.GetProductAccess(licenseeId, language, FunctionNames.LiveReport);

        private DateTime GetBalanceUpDay()
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            return balanceUp != null ? balanceUp.BalanceUpDay : DateTime.Now;
        }
    }
}