// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerPendingFunds.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// The customer pending fund controller.
    /// </summary>
    public partial class ReportController
    {
        #region Web Controller

        /// <summary>
        /// The customer pending fund report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerPendingFunds()
        {
            var metaModel = new CustomerPendingFundsMetaModel
                {
                    LicenseeId = this.SessionService.LicenseeSession.LicenseeId,
                    UserId = SessionService.UserSession.UserId
                };
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";

            var productAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                                 {
                                                     Code = i.ProductCode,
                                                     Name = i.ProductName,
                                                     Status = i.ProductCode == Product.Sb
                                                 }).ToList();

            metaModel.ProductOptions = productOption;

            var currencyList = SessionService.UserSession.CurrencyList;
            var model = new CustomerPendingFundsViewModel
                {
                    ShowEmptyContent = true,
                    DataList = new List<CustPendingFunds>(),
                    TotalData = new CustPendingFunds(),
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                    CurrencyList = currencyList,
                    Param = metaModel,
                    TotalRows = 0,
                };

            return View("~/Views/Report/CustomerPendingFunds/Main.cshtml", model);
        }

        /// <summary>
        /// The customer pending fund report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "None")]
        [FunctionAuthorization]
        [ValidateInput(false)]
        public ActionResult CustomerPendingFunds(CustomerPendingFundsMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.LicenseeSession.UserId;

            var currencyList = SessionService.UserSession.CurrencyList;
            var totalData = new CustPendingFunds();
            var totalRows = 0;

            var model = new CustomerPendingFundsViewModel
                {
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                    CurrencyList = currencyList,
                    Param = metaModel,
                    TotalRows = totalRows,
                };

            var dataList = _custPendingFundsService.GetDataList(metaModel, ref totalData, ref totalRows);            
            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            model.TotalRows = totalRows;
            model.DataList = dataList;
            model.TotalData = totalData;

            if (metaModel.IsExportExcel)
            {
                metaModel.PageSize = PageSizeExcel;
                return CustomerPendingFundsExcel(model);
            }

            model.DataList = dataList;
            model.TotalData = totalData;

            return View("~/Views/Report/CustomerPendingFunds/Main.cshtml", model);
        }

        #endregion Web Controller

        #region Export excel

        /// <summary>
        /// Customer pending fund excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult CustomerPendingFundsExcel(CustomerPendingFundsViewModel viewModel)
        {
            const string FileName = "CustomerPendingFunds";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerPendingFunds.xls");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException(templatePath);
            }

            var convertingOption = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            var custPendingFundsExcel = new CustomerPendingFundsExcel(templatePath, FileName, convertingOption, viewModel);
            custPendingFundsExcel.Export();

            return new ExcelResult(custPendingFundsExcel.Workbook, FileName);
        }

        #endregion Export excel
    }
}