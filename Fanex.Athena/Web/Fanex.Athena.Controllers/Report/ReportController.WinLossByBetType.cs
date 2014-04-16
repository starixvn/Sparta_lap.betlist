// -----------------------------------------------------------------------
// <copyright file="ReportController.WinLossByBetType.cs" company="Nexcel">
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
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     Win Loss By Bet Type Controller.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// Win loss by bet type report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult WinLossByBetType()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var metaModel = new WinLossByBetTypeMetaModel
                {
                    BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay,
                    LicenseeId = licenseeSession.LicenseeId,
                    UserId = SessionService.UserSession.UserId
                };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new WinLossByBetTypeViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    BalanceUpdate = _balanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language),
                };

            ViewBag.IsShowCustComm = SessionService.LicenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        /// <summary>
        /// Win loss by bet type report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult WinLossByBetType(WinLossByBetTypeMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;

            var reportData = _winLossByBetTypeService.GetDataList(metaModel);

            reportData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var total = GetWinLossByBetTypeTotal(reportData);
            var model = new WinLossByBetTypeViewModel
                {
                    Param = metaModel,
                    MainResult = reportData,
                    BalanceUpdate = _balanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language),
                    ColumnCount = GetMatchWinlossColumnCount(licenseeSession),
                    Total = total
                };

            ViewBag.IsShowCustComm = SessionService.LicenseeSession.IsAllowedCustomerComm;

            return metaModel.IsExportExcel ?
                WinLossByBetTypeExcel(model) :
                View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        #region Excel

        /// <summary>
        /// Win loss by bet type excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult WinLossByBetTypeExcel(WinLossByBetTypeViewModel viewModel)
        {
            const string FileName = "WinLossByBetType";

            var templatePath = Server.MapPath("~/App_Data/Template/Reports/WinLossByBetType.xls");

            var currencyConvert = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            var productAccess = _productService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language);

            var showingOption = new ShowingOption
                {
                    RbVisible = productAccess.IsAllowRb,
                    BaVisible = productAccess.IsAllowBa,
                    NgVisible = productAccess.IsAllowNg,
                    SbVisible = productAccess.IsAllowSB,
                    LcVisible = productAccess.IsAllowLc,
                    VsVisible = productAccess.IsAllowVs,
                    CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            var winLossByBetTypeExcel = new WinLossByBetTypeExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);

            winLossByBetTypeExcel.Export();

            return new ExcelResult(winLossByBetTypeExcel.Workbook, FileName);
        }

        #endregion Excel

        #region Get total

        /// <summary>
        /// Calculate total for report.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total object.</returns>
        private static WinLossByBetType GetWinLossByBetTypeTotal(IList<WinLossByBetType> dataList)
        {
            return new WinLossByBetType
            {
                TotalTurnover = dataList.Sum(item => item.Turnover),
                TotalWinLoss = dataList.Sum(item => item.CustomerWinLoss),
                TotalCustomerComm = dataList.Sum(item => item.CustomerComm),
                TotalLicenseeWl = dataList.Sum(item => item.LicenseeWL),
                CustomerWinLoss = dataList.Sum(item => item.CustomerWinLoss),
                CustomerComm = dataList.Sum(item => item.CustomerComm),
                TotalBaCommission = dataList.Sum(item => item.BACommission),
                TotalAmtDueToBa = dataList.Sum(item => item.AmtDueToBA),
                TotalRacingRevenue = dataList.Sum(item => item.RacingRevenue),
                TotalRacingComm = dataList.Sum(item => item.RbComm),
                TotalAmtDueToRacing = dataList.Sum(item => item.AmtDueToRacing)
            };
        }

        #endregion Get total
    }
}