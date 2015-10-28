// <copyright file="ReportController.CustomerWinloss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    #region

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    #endregion

    /// <summary>
    /// The customer win-loss controller.
    /// </summary>
    public partial class ReportController
    {
        private const string CustomerWinLossName = "CustomerWinLoss";

        /// <summary>
        /// The customer win-loss.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerWinLoss()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new CustomerWinLossMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                DrawFilter = DrawFilterType.Normal
            };

            SetConvetedUSDCurrencyName(metaModel);
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, CustomerWinLossName);
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

        /// <summary>
        /// Customer win-loss history report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerWinlossHistory()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var historyParam = new CustomerWinLossHistoryMetaModel
                {
                    LicenseeId = licenseeSession.LicenseeId,
                    DrawFilter = DrawFilterType.Normal
                };

            SetConvetedUSDCurrencyName(historyParam);

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, CustomerWinLossName);
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

        /// <summary>
        /// The customer win-loss report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult CustomerWinLoss(CustomerWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            var totalData = new CustomerWinLoss();
            var totalRows = 0;
            var custWinlossService = new CustomerWinlossService();
            var dataList = custWinlossService.GetDataList(metaModel, totalData, out totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            
            var model = new CustomerWinLossViewModel
                {
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                    CurrencyList = GetUserCurrencies(),
                    Param = metaModel,
                    IsHistory = false,
                };

            model.TotalRows = totalRows;
            model.DataList = dataList;
            model.TotalData = totalData;

            return metaModel.IsExportExcel
                       ? CustomerWinLossExcel(model)
                       : View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }

        /// <summary>
        /// The customer win-loss history report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult CustomerWinlossHistory(CustomerWinLossHistoryMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            metaModel.FromDate = new DateTime(metaModel.Year, metaModel.Month, metaModel.FromDay);
            metaModel.ToDate = new DateTime(metaModel.Year, metaModel.Month, metaModel.ToDay);

            var totalData = new CustomerWinLoss();
            var totalRows = 0;
            var custWinlossService = new CustomerWinlossService();
            var dataList = custWinlossService.GetDataList(metaModel, totalData, out totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var model = new CustomerWinLossViewModel
                {
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
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

                return CustomerWinLossHistoryExcel(model);
            }

            return this.View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }

        #region Export excel

        /// <summary>
        /// The customer win-loss excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult CustomerWinLossExcel(CustomerWinLossViewModel viewModel)
        {
            const string FileName = "CustomerWinLoss";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerWinLoss.xls");
            var productAccess = _productService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language);

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

        /// <summary>
        /// The customer win-loss history excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult CustomerWinLossHistoryExcel(CustomerWinLossViewModel viewModel)
        {
            const string FileName = "CustomerWinLoss";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerWinLoss.xls");
            var productAccess = _productService.GetProductAccess(viewModel.HistoryParam.LicenseeId, SessionService.UserSession.Language);

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

        #endregion Export excel
    }
}