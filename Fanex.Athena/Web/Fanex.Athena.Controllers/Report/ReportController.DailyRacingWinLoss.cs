// -----------------------------------------------------------------------
// <copyright file="ReportController.DailyRacingWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    #region

    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// The daily racing win-loss.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult DailyRacingWinLoss()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var metaModel = new DailyRacingWinLossMetaModel
                {
                    LicenseeId = licenseeSession.LicenseeId,
                    UserId = licenseeSession.UserId,
                    BalanceUpDay = this._balanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString()
                };
           
            SetConvetedUSDCurrencyName(metaModel);
           
            var model = new DailyRacingWinLossViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                };

            return View("~/Views/Report/DailyRacingWinLoss/Main.cshtml", model);
        }

        /// <summary>
        /// The daily racing win-loss.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult DailyRacingWinLoss(DailyRacingWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;
            metaModel.BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString();
            var dailyRacingWlData = _dailyRacingWinLossService.GetDailyRacingWinLossData(metaModel);

            var model = new DailyRacingWinLossViewModel
                {
                    Param = metaModel,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    DailyRacingWinLossData = dailyRacingWlData
                };

            return metaModel.IsExportExcel
                       ? DailyRacingWinLossExcel(model)
                       : View("~/Views/Report/DailyRacingWinLoss/Main.cshtml", model);
        }

        #region Export Excel

        /// <summary>
        /// The daily racing win-loss excel.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The action result.</returns>
        private ActionResult DailyRacingWinLossExcel(DailyRacingWinLossViewModel viewModel)
        {
            const string FileName = "DailyRacingWinLossReport";

            var templatePath = this.SessionService.LicenseeSession.LicenseeId == 1
                                   ? Server.MapPath("~/App_Data/Template/Reports/DailyRacingWinLoss12Bet.xls")
                                   : Server.MapPath("~/App_Data/Template/Reports/DailyRacingWinLoss.xls");

            var currencyConvert = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            var showingOption = new ShowingOption
                {
                    CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            // Export excel
            if (this.SessionService.LicenseeSession.LicenseeId == 1)
            {
                var racingWinLossExcel12Bet = new DailyRacingWinLoss12BetExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);
                racingWinLossExcel12Bet.Export();

                return new ExcelResult(racingWinLossExcel12Bet.Workbook, FileName);
            }

            var dailyRacingWinLossExcel = new DailyRacingWinLossExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);

            dailyRacingWinLossExcel.Export();

            return new ExcelResult(dailyRacingWinLossExcel.Workbook, FileName);
        }

        #endregion
    }
}