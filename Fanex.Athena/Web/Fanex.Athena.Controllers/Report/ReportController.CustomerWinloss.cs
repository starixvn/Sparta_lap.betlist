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
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    #endregion

    /// <summary>
    /// The customer win-loss controller.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// The customer win-loss.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerWinLoss()
        {            
            var metaModel = new CustomerWinLossMetaModel
            {
                LicenseeId = this.SessionService.LicenseeSession.LicenseeId,
                DrawFilter = DrawFilterType.Normal
            };
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
            var productAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreKeno);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                                 {
                                                     Code = i.ProductCode,
                                                     Name = i.ProductName,
                                                     Status = i.ProductCode == Product.Sb
                                                 }).ToList();

            metaModel.ProductOptions = productOption;
            var currencyList = SessionService.UserSession.CurrencyList;

            var model = new CustomerWinLossViewModel
                {
                    ShowEmptyContent = true,
                    ProductAccess = productAccess,
                    Param = metaModel,
                    IsHistory = false
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
            var historyParam = new CustomerWinLossHistoryMetaModel
                {
                    LicenseeId = this.SessionService.LicenseeSession.LicenseeId,

                    DrawFilter = DrawFilterType.Normal
                };

            SetConvetedUSDCurrencyName(historyParam);

            var productAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreKeno);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                                 {
                                                     Code = i.ProductCode,
                                                     Name = i.ProductName,
                                                     Status = i.ProductCode == Product.Sb
                                                 }).ToList();

            historyParam.ProductOptions = productOption;

            var currencyList = SessionService.UserSession.CurrencyList;
            var model = new CustomerWinLossViewModel
                {
                    ShowEmptyContent = true,
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = productAccess,
                    CurrencyList = currencyList,
                    HistoryParam = historyParam,
                    IsHistory = true
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
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            var currencyList = SessionService.UserSession.CurrencyList;
            var totalData = new CustomerWinLoss();
            var totalRows = 0;
            var dataList = _custWinlossService.GetDataList(metaModel, ref totalData, ref totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var model = new CustomerWinLossViewModel
                {
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreKeno),
                    CurrencyList = currencyList,
                    Param = metaModel,
                    IsHistory = false
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
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.FromDate = new DateTime(metaModel.Year, metaModel.Month, metaModel.FromDay);
            metaModel.ToDate = new DateTime(metaModel.Year, metaModel.Month, metaModel.ToDay);

            var currencyList = SessionService.UserSession.CurrencyList;
            var totalData = new CustomerWinLoss();
            var totalRows = 0;
            var dataList = _custWinlossService.GetDataList(metaModel, ref totalData, ref totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var model = new CustomerWinLossViewModel
                {
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreKeno),
                    CurrencyList = currencyList,
                    HistoryParam = metaModel,
                    TotalRows = totalRows,
                    IsHistory = true,
                    DataList = dataList,
                    TotalData = totalData
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
            var productAccess = _productService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language, IgnoreKeno);

            var convertingOption = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            var showingOption = new ShowingOption
                {
                    BaVisible = productAccess.IsAllowBa,
                    RbVisible = productAccess.IsAllowRb,
                    NgVisible = productAccess.IsAllowNg,
                    LcVisible = productAccess.IsAllowLc,
                    SbVisible = productAccess.IsAllowSB,
                    VsVisible = productAccess.IsAllowVs,
                    CsVisible = productAccess.IsAllowCs,
                    BgVisible = productAccess.IsAllowBg,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
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
            var productAccess = _productService.GetProductAccess(viewModel.HistoryParam.LicenseeId, SessionService.UserSession.Language, IgnoreKeno);

            var convertingOption = new ConvertingOption
                {
                    IsConvert = viewModel.HistoryParam.IsConvert,
                    CurrencyName = viewModel.HistoryParam.CurrencyName,
                    BaseCurrency = viewModel.HistoryParam.BaseCurrency
                };

            var showingOption = new ShowingOption
                {
                    BaVisible = productAccess.IsAllowBa,
                    RbVisible = productAccess.IsAllowRb,
                    NgVisible = productAccess.IsAllowNg,
                    LcVisible = productAccess.IsAllowLc,
                    SbVisible = productAccess.IsAllowSB,
                    VsVisible = productAccess.IsAllowVs,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
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