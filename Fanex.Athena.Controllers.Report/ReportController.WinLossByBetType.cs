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
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
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
            var userSession = SessionService.UserSession;
            var metaModel = new WinLossByBetTypeMetaModel
                {
                    BalanceUpDay = BalanceUpService.BalanceUpInfo.BalanceUpDay,
                    LicenseeId = licenseeSession.LicenseeId,
                    UserId = userSession.UserId
                };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new WinLossByBetTypeViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    BalanceUpdate = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    CurrencyList = GetUserCurrencies(),
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        /// <summary>
        /// Win loss by bet type report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult WinLossByBetType(WinLossByBetTypeMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;
            var winLossByBetTypeService = new WinLossByBetTypeService();
            var reportData = winLossByBetTypeService.GetDataList(metaModel);

            reportData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var total = GetWinLossByBetTypeTotal(reportData);
            var model = new WinLossByBetTypeViewModel
                {
                    Param = metaModel,
                    MainResult = reportData,
                    BalanceUpdate = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    CurrencyList = GetUserCurrencies(),
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
                    ColumnCount = GetMatchWinlossColumnCount(licenseeSession),
                    Total = total
                };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return metaModel.IsExportExcel ?
                WinLossByBetTypeExcel(model) :
                View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        /// <summary>
        /// Calculate total for report.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total object.</returns>
        private static WinLossByBetType GetWinLossByBetTypeTotal(IList<WinLossByBetType> dataList)
        {
            return new WinLossByBetType
            {
                Turnover = dataList.Sum(item => item.Turnover),
                CustomerWinLoss = dataList.Sum(item => item.CustomerWinLoss),
                CustomerComm = dataList.Sum(item => item.CustomerComm),
                BACommission = dataList.Sum(item => item.BACommission),
                AmtDueToBA = dataList.Sum(item => item.AmtDueToBA),
                RacingRevenue = dataList.Sum(item => item.RacingRevenue),
                RBComm = dataList.Sum(item => item.RBComm),
                AmtDueToRacing = dataList.Sum(item => item.AmtDueToRacing),
                LicenseeWL = dataList.Sum(i => i.LicenseeWL),
            };
        }

        /// <summary>
        /// Win loss by bet type excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        private ActionResult WinLossByBetTypeExcel(WinLossByBetTypeViewModel viewModel)
        {
            const string FileName = "WinLossByBetType";

            var templatePath = Server.MapPath("~/App_Data/Template/Reports/WinLossByBetType.xls");

            var currencyConvert = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var productAccess = ProductService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language);

            var showingOption = new ShowingOption
            {
                RBVisible = productAccess.IsAllowRB,
                BAVisible = productAccess.IsAllowBA,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB,
                LCVisible = productAccess.IsAllowLC,
                VSVisible = productAccess.IsAllowVS,
                CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            var winLossByBetTypeExcel = new WinLossByBetTypeExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);

            winLossByBetTypeExcel.Export();

            return new ExcelResult(winLossByBetTypeExcel.Workbook, FileName);
        }
    }
}