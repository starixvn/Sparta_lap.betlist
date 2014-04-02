// -----------------------------------------------------------------------
// <copyright file="WareHouseController.TopCustomer.cs" company="nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Warehouse;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class WareHouseController
    {
        private ISportTypeService _sportTypeService;

        /// <summary>
        ///     Get sport list.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>The sport list.</returns>
        [HttpPost]
        public ActionResult GetSportList(DateTime fromDate, DateTime toDate)
        {
            _sportTypeService = new SportTypeService(this._balancUpService.BalanceUpInfo);
            var sportList = _sportTypeService.GetAllWHSports();
            return View("~/Views/WareHouse/TopCustomer/AjaxTemplate/SportList.cshtml", sportList);
        }

        /// <summary>
        /// The top customer report.
        /// </summary>
        /// <returns>
        ///    The action result.
        /// </returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult TopCustomer()
        {
            _sportTypeService = new SportTypeService(this._balancUpService.BalanceUpInfo);

            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new TopCustomerMetaModel { LicenseeId = licenseeId };
            metaModel.FromDate = metaModel.ToDate = _balancUpService.BalanceUpInfo.MoveBetsDay;
            var currencyList = SessionService.UserSession.CurrencyList;
            var sportList = _sportTypeService.GetAllWHSports();
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);

            var model = new TopCustomerViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    ProductAccess = productAccess,
                    CurrencyList = currencyList,
                    SportList = sportList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
                };

            ViewBag.MaxDate = _balancUpService.BalanceUpInfo.MoveBetsDay;
            return View("~/Views/WareHouse/TopCustomer/Main.cshtml", model);
        }

        /// <summary>
        ///     Warehouse top customer export excel.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult ExportExcel(TopCustomerMetaModel metaModel)
        {
            const string FileName = "TopCustomer";
            const string FormatCurrency = "{0} {1}";
            const string FormatTitle = "{0} {1} {2} {3} {4} --> {5} - {6}";
            var templatePath = Server.MapPath("~/App_Data/Template/DataWareHouse/TopCustomer.xls");
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = _productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language);
            var showingOption = new ShowingOption
                {
                    BaVisible = productAccess.IsAllowBa,
                    RbVisible = productAccess.IsAllowRb,
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                    NgVisible = productAccess.IsAllowNg,
                    SbVisible = productAccess.IsAllowSB
                };
            var currencyConvert = new ConvertingOption
                {
                    IsConvert = metaModel.IsConvert,
                    BaseCurrency = metaModel.BaseCurrency,
                    CurrencyName = metaModel.CurrencyName
                };
            var viewByCurrency = metaModel.IsConvert
                                     ? string.Format(FormatCurrency, ViewResources.Viewin, metaModel.BaseCurrency.ToUpper())
                                     : string.Format(FormatCurrency, ViewResources.Viewin, metaModel.CurrencyName);
            var titleText = string.Format(
                FormatTitle,
                ViewResources.Top,
                metaModel.TopRow,
                ViewResources.Customer,
                ViewResources.From,
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                viewByCurrency);

            var exportData = _topCustomerService.GetDataForExcel(metaModel);

            var topCustExcel = new TopCustomer(templatePath, FileName, currencyConvert, showingOption, exportData)
                {
                    ReportTitle = titleText
                };

            topCustExcel.Export();
            return new ExcelResult(topCustExcel.Workbook, FileName);
        }

        /// <summary>
        ///     Warehouse top customer get data.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <returns>
        ///   The  html string.
        /// </returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult TopCustomer(TopCustomerMetaModel metaModel)
        {
            _sportTypeService = new SportTypeService(this._balancUpService.BalanceUpInfo);

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel);
            }

            var licenseeSession = SessionService.LicenseeSession;

            /*Set default values*/
            var model = new TopCustomerViewModel
                {
                    Param = metaModel,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    ProductAccess = _productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language),
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    SportList = _sportTypeService.GetAllWHSports(),
                };

            var totalValues = new Dictionary<string, decimal>();

            ViewBag.BalanceUpDay = _balancUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString();
            model.MainResult = _topCustomerService.GetTopCustomerList(metaModel, ref totalValues);
            model.TotalValues = totalValues;

            ViewBag.MaxDate = _balancUpService.BalanceUpInfo.MoveBetsDay;
            return View("~/Views/WareHouse/TopCustomer/Main.cshtml", model);
        }
    }
}