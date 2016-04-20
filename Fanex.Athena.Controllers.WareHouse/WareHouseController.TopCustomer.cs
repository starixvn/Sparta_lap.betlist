// -----------------------------------------------------------------------
// <copyright file="WareHouseController.TopCustomer.cs" company="nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Athena.Models.ViewModel.Warehouse;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class WareHouseController
    {
        private ISportTypeService _sportTypeService;

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
            _sportTypeService = new SportTypeService();

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new TopCustomerMetaModel { LicenseeId = licenseeId };
            metaModel.FromDate = metaModel.ToDate = _balancUpService.BalanceUpInfo.MoveBetsDay;
            var currencyList = userSession.CurrencyList;
            var sportList = _sportTypeService.GetAllWHSports();
            var productAccess = _productService.GetProductAccess(licenseeId, userSession.Language);

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
            var userSession = SessionService.UserSession;
            const string FileName = "TopCustomer";
            const string FormatTitle = "{0} {1} {2} {3} {4} --> {5} - {6}";
            metaModel.UserId = userSession.UserId;
            var templatePath = Server.MapPath("~/App_Data/Template/DataWareHouse/TopCustomer.xls");
            var productAccess = _productService.GetProductAccess(metaModel.LicenseeId, userSession.Language);
            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB
            };
            var currencyConvert = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };
            var viewByCurrency =
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
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
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            _sportTypeService = new SportTypeService();

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel);
            }

            /*Set default values*/
            var model = new TopCustomerViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = _productService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
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