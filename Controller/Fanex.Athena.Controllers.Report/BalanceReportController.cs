// -----------------------------------------------------------------------
// <copyright file="ReportController.Balance.cs" company="">
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
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportBalance)]
    public class BalanceReportController : ReportBaseController
    {
        private readonly ICreditBalanceService _creditBalanceService;

        public BalanceReportController()
        {
            _creditBalanceService = new CreditBalanceService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new BalanceMetaModel { LicenseeId = SessionService.LicenseeSession.LicenseeId };
            SetConvetedUSDCurrencyName(metaModel);
            return Index(metaModel);
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "None")]
        [CustomValidateAntiForgery]
        public ActionResult Index(BalanceMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            metaModel.MoveBetsDay = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            var reportData = _creditBalanceService.GetReportData(metaModel);

            var viewData = new BalanceViewModel
            {
                MainData = reportData,
                Param = metaModel,
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
            };

            if (metaModel.IsExportExcel)
            {
                return Excel(viewData);
            }

            return View("~/Views/Report/Balance/Main.cshtml", viewData);
        }

        private ActionResult Excel(BalanceViewModel model)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            const string FileName = "Balance";
            var templatePath = Server.MapPath("~/App_Data/Template/Reports/Balance.xls");
            var productAccess = this.ProductService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);

            var convertingOption = new ConvertingOption
            {
                IsConvert = model.Param.IsConvert,
                BaseCurrency = model.Param.BaseCurrency,
                CurrencyName = model.Param.CurrencyName
            };

            var showingOption = new ShowingOption
            {
                RBVisible = productAccess.IsAllow(ProductTypes.Racing),
                BAVisible = productAccess.IsAllow(ProductTypes.BA),
                NGVisible = productAccess.IsAllow(ProductTypes.NumberGame),
                SBVisible = productAccess.IsAllow(ProductTypes.Sportsbook),
                LCVisible = productAccess.IsAllow(ProductTypes.LiveCasino),
                VSVisible = productAccess.IsAllow(ProductTypes.VirtualSports),
                CSVisible = productAccess.IsAllow(ProductTypes.RngCasino),
                BGVisible = productAccess.IsAllow(ProductTypes.Bingo),
                KNVisible = productAccess.IsAllow(ProductTypes.Keno),
                BTVisible = productAccess.IsAllow(ProductTypes.BetTrade),
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var balance = new BalanceExcel(
                templatePath,
                showingOption,
                convertingOption,
                model.MainData,
                model.Param.FromDate.ToString("MM - yyyy"))
            { ProductOptions = model.Param.ProductOptions };
            balance.Export();

            return new ExcelResult(balance.Workbook, FileName);
        }
    }
}