// -----------------------------------------------------------------------
// <copyright file="WareHouseController.WinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Warehouse;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class WareHouseController
    {
        /// <summary>
        ///     Report:warehouse/win-loss.
        ///     First load.
        /// </summary>
        /// <returns>
        ///     The action result.
        /// </returns>
        [HttpGet]
        public ActionResult WinLoss()
        {
            _licenseeList = _licenseeService.GetByUser(SessionService.LicenseeSession.UserId);

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var currencyList = SessionService.UserSession.CurrencyList;
            var metaModel = new WinLossMetaModel { LicenseeId = licenseeId };
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language);

            var model = new WinLossViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    LicenseeList = _licenseeList,
                    CurrencyList = currencyList,
                    ProductAccess = productAccess,
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            return View("~/Views/WareHouse/WinLoss/Main.cshtml", model);
        }

        /// <summary>
        ///     Warehouse win-loss export excel.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult ExportExcel(WinLossMetaModel metaModel)
        {
            const string FileName = "WinLoss";
            const string FormatCurrency = "{0} {1}";
            const string FormatTitle = "{0} {1} {2} {3} --> {4} - {5}";
            var templatePath = Server.MapPath("~/App_Data/Template/DataWareHouse/WinLoss.xls");
            var productAccess = _productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language);
            var showingOption = new ShowingOption
                {
                    BaVisible = productAccess.IsAllowBa,
                    RbVisible = productAccess.IsAllowRb,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    NgVisible = productAccess.IsAllowNg,
                    SbVisible = productAccess.IsAllowSB
                };
            var viewByCurrency = metaModel.IsConvert
                                     ? string.Format(FormatCurrency, ViewResources.Viewin, metaModel.BaseCurrency.ToUpper())
                                     : string.Format(FormatCurrency, ViewResources.Viewin, metaModel.CurrencyName);
            var groupingList = GroupingMetaModel.GetCollection();
            var titleText = string.Format(
                FormatTitle,
                ViewResources.ReportBy,
                 groupingList[metaModel.ReportBy.ToString(CultureInfo.InvariantCulture)],
                  ViewResources.From,
                 metaModel.FromDate.ToString("MM/dd/yyyy"),
                 metaModel.ToDate.ToString("MM/dd/yyyy"),
                 viewByCurrency);
            var licenseeList = GetLicenseeList(metaModel.LicenseeId);
            _licenseeList = _licenseeService.GetByUser(SessionService.LicenseeSession.UserId);
            var exportData = new DataTable();
            if (_licenseeList.Any(a => a.Id == metaModel.LicenseeId))
            {
                exportData = _winLossService.GetDataForExcel(metaModel, licenseeList);
            }

            var currencyConvert = new ConvertingOption
                {
                    IsConvert = metaModel.IsConvert,
                    BaseCurrency = metaModel.BaseCurrency,
                    CurrencyName = metaModel.CurrencyName
                };
            var winLossExcel = new WinLoss(templatePath, FileName, currencyConvert, showingOption, exportData)
                {
                    GroupingId = metaModel.ReportBy,
                    ReportTitle = titleText
                };

            winLossExcel.Export();
            return new ExcelResult(winLossExcel.Workbook, FileName);
        }

        /// <summary>
        ///     Warehouse win-loss get data.
        /// </summary>
        /// <param name="metaModel">
        ///     The metal model.
        /// </param>
        /// <returns>
        ///     The html string.
        /// </returns>
        [HttpPost]
        public ActionResult WinLoss(WinLossMetaModel metaModel)
        {
            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel);
            }

            var licenseeList = GetLicenseeList(metaModel.LicenseeId);

            _licenseeList = _licenseeService.GetByUser(SessionService.LicenseeSession.UserId);

            /*Set default values*/
            var model = new WinLossViewModel
                {
                    Param = metaModel,
                    LicenseeList = _licenseeList,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    ProductAccess = _productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language),
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            if (_licenseeList.Any(a => a.Id == metaModel.LicenseeId))
            {
                IDictionary<string, decimal> totalValues = new Dictionary<string, decimal>();

                model.MainResult = _winLossService.GetDataList(metaModel, licenseeList, ref totalValues);
                model.TotalValues = totalValues;
            }
            else
            {
                /*Set default values when user edit value in brower*/
                model.MainResult = new List<Models.DTO.Warehouse.WinLoss>();
                model.TotalValues = new Dictionary<string, decimal>();
            }

            return View("~/Views/WareHouse/WinLoss/Main.cshtml", model);
        }
    }
}