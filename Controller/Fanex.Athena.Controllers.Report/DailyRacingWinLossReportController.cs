// -----------------------------------------------------------------------
// <copyright file="ReportController.DailyRacingWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportDailyRacingWinLoss)]
    public class DailyRacingWinLossReportController : ReportBaseController
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IDailyRacingWinLossService _dailyRacingWinLossService;

        public DailyRacingWinLossReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IDailyRacingWinLossService dailyRacingWinLossService)
            : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _dailyRacingWinLossService = dailyRacingWinLossService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new DailyRacingWinLossMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId,
                BalanceUpDay = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay.ToShortDateString()
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new DailyRacingWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
            };

            return View("~/Views/Report/DailyRacingWinLoss/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(DailyRacingWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.BalanceUpDay = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay.ToShortDateString();
            var dailyRacingWlData = _dailyRacingWinLossService.GetDailyRacingWinLossData(metaModel);

            var model = new DailyRacingWinLossViewModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                Param = metaModel,
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
            var templatePath = licenseeSession.LicenseeId == LicenseeIdentity.OneTwoBet
                                   ? HostingEnvironment.MapPath("~/App_Data/Template/Reports/DailyRacingWinLoss12Bet.xls")
                                   : HostingEnvironment.MapPath("~/App_Data/Template/Reports/DailyRacingWinLoss.xls");

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

            if (licenseeSession.LicenseeId == LicenseeIdentity.OneTwoBet)
            {
                var excel12bet = Create12BetExcel(templatePath, viewModel, currencyConvert, showingOption);
                excel12bet.Export();

                return new ExcelResult(excel12bet.Workbook, FileName);
            }

            var excel = CreateExcel(templatePath, viewModel, currencyConvert, showingOption);

            excel.Export();

            return new ExcelResult(excel.Workbook, FileName);
        }

        protected internal virtual DailyRacingWinLossExcel CreateExcel(
            string templatePath,
            DailyRacingWinLossViewModel viewModel,
            ConvertingOption currencyConvert,
            ShowingOption showingOption)
        {
            return new DailyRacingWinLossExcel(templatePath, currencyConvert, showingOption, viewModel);
        }

        protected internal virtual DailyRacingWinLoss12BetExcel Create12BetExcel(
            string templatePath,
            DailyRacingWinLossViewModel viewModel,
            ConvertingOption currencyConvert,
            ShowingOption showingOption)
        {
            return new DailyRacingWinLoss12BetExcel(templatePath, currencyConvert, showingOption, viewModel);
        }
    }
}