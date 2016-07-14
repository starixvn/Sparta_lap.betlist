// -----------------------------------------------------------------------
// <copyright file="ReportController.DailyRacingWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportDailyRacingWinLoss)]
    public class DailyRacingWinLossReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var metaModel = new DailyRacingWinLossMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = licenseeSession.UserId,
                BalanceUpDay = this.BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString()
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

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(DailyRacingWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;
            metaModel.BalanceUpDay = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString();
            var dailyRacingWinLossService = new DailyRacingWinLossService();
            var dailyRacingWlData = dailyRacingWinLossService.GetDailyRacingWinLossData(metaModel);

            var model = new DailyRacingWinLossViewModel
            {
                Param = metaModel,
                LicenseeTypeList = licenseeSession.LicenseeType,
                CurrencyList = SessionService.UserSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                DailyRacingWinLossData = dailyRacingWlData
            };

            return metaModel.IsExportExcel
                       ? Excel(model)
                       : View("~/Views/Report/DailyRacingWinLoss/Main.cshtml", model);
        }

        private ActionResult Excel(DailyRacingWinLossViewModel viewModel)
        {
            const string FileName = "DailyRacingWinLossReport";

            var licenseeSession = SessionService.LicenseeSession;
            var templatePath = licenseeSession.LicenseeId == 1
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
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            // Export excel
            if (licenseeSession.LicenseeId == 1)
            {
                var racingWinLossExcel12Bet = new DailyRacingWinLoss12BetExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);
                racingWinLossExcel12Bet.Export();

                return new ExcelResult(racingWinLossExcel12Bet.Workbook, FileName);
            }

            var dailyRacingWinLossExcel = new DailyRacingWinLossExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);

            dailyRacingWinLossExcel.Export();

            return new ExcelResult(dailyRacingWinLossExcel.Workbook, FileName);
        }
    }
}