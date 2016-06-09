// <copyright file="ReportController.CustomerWinloss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    public class CustomerWinLossReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new CustomerWinLossMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                DrawFilter = DrawFilterType.Normal,
                IsExcludedTestData = true
            };

            SetConvetedUSDCurrencyName(metaModel);
            var productAccess = ProductService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerWinLoss);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;
            var model = new CustomerWinLossViewModel
            {
                ShowEmptyContent = true,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = productAccess,
                CurrencyList = GetUserCurrencies(),
                Param = metaModel,
                IsHistory = false,
            };

            return View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult History()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var historyParam = new CustomerWinLossHistoryMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                DrawFilter = DrawFilterType.Normal,
                IsExcludedTestData = true
            };

            SetConvetedUSDCurrencyName(historyParam);

            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.CustomerWinLoss);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            historyParam.ProductOptions = productOption;

            var model = new CustomerWinLossViewModel
            {
                ShowEmptyContent = true,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = productAccess,
                CurrencyList = GetUserCurrencies(),
                HistoryParam = historyParam,
                IsHistory = true,
            };

            return View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }

        [HttpPost]
        public ActionResult Index(CustomerWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            CustomerWinLoss totalData;
            int totalRows;
            var custWinlossService = new CustomerWinlossService();
            var dataList = custWinlossService.GetDataList(metaModel, out totalData, out totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var model = new CustomerWinLossViewModel
            {
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                CurrencyList = GetUserCurrencies(),
                Param = metaModel,
                IsHistory = false,
            };

            model.TotalRows = totalRows;
            model.DataList = dataList;
            model.TotalData = totalData;

            return metaModel.IsExportExcel
                       ? Excel(model)
                       : View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }

        [HttpPost]
        public ActionResult History(CustomerWinLossHistoryMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            metaModel.FromDate = new DateTime(metaModel.Year, metaModel.Month, metaModel.FromDay);
            metaModel.ToDate = new DateTime(metaModel.Year, metaModel.Month, metaModel.ToDay);

            CustomerWinLoss totalData;
            int totalRows;
            var custWinlossService = new CustomerWinlossService();
            var dataList = custWinlossService.GetDataList(metaModel, out totalData, out totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var model = new CustomerWinLossViewModel
            {
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                CurrencyList = GetUserCurrencies(),
                HistoryParam = metaModel,
                TotalRows = totalRows,
                IsHistory = true,
                DataList = dataList,
                TotalData = totalData,
            };

            if (metaModel.IsExportExcel)
            {
                model.Param = new CustomerWinLossMetaModel
                {
                    ToDate = model.HistoryParam.ToDate,
                    FromDate = model.HistoryParam.FromDate,
                    CurrencyId = model.HistoryParam.CurrencyId
                };

                return HistoryExcel(model);
            }

            return this.View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }

        [HttpPost]
        public ActionResult Excel(CustomerWinLossViewModel viewModel)
        {
            const string FileName = "CustomerWinLoss";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language);

            var convertingOption = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                CSVisible = productAccess.IsAllowCS,
                BGVisible = productAccess.IsAllowBG,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var custWinlossExcel = new CustomerWinLossExcel(
                templatePath,
                FileName,
                showingOption,
                convertingOption,
                viewModel);

            custWinlossExcel.ProductOptions = viewModel.Param.ProductOptions;
            custWinlossExcel.Export();

            return new ExcelResult(custWinlossExcel.Workbook, FileName);
        }

        [HttpPost]
        public ActionResult HistoryExcel(CustomerWinLossViewModel viewModel)
        {
            const string FileName = "CustomerWinLoss";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(viewModel.HistoryParam.LicenseeId, SessionService.UserSession.Language);

            var convertingOption = new ConvertingOption
            {
                IsConvert = viewModel.HistoryParam.IsConvert,
                CurrencyName = viewModel.HistoryParam.CurrencyName,
                BaseCurrency = viewModel.HistoryParam.BaseCurrency
            };

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var custWinlossExcel = new CustomerWinLossExcel(
                templatePath,
                FileName,
                showingOption,
                convertingOption,
                viewModel);

            custWinlossExcel.ProductOptions = viewModel.HistoryParam.ProductOptions;
            custWinlossExcel.Export();

            return new ExcelResult(custWinlossExcel.Workbook, FileName);
        }
    }
}