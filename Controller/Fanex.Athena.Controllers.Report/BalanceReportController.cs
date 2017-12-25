// -----------------------------------------------------------------------
// <copyright file="ReportController.Balance.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Report.DTO;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportBalance)]
    public class BalanceReportController : ReportBaseController
    {
        private readonly IBalanceDataAccess _balanceDataAccess;

        public BalanceReportController() : this(null, null, null, null, null, null)
        {
        }

        public BalanceReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceDataAccess balanceDataAccess,
            IClientViewHelper clientViewHelper)
        : base(languageService, sessionService, appSetting, productService, clientViewHelper)
        {
            _balanceDataAccess = balanceDataAccess ?? new BalanceDataAccess();
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
            var reportData = metaModel.IsValid
                       ? _balanceDataAccess.GetReportData(metaModel)
                       : new Balance();

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

        protected internal virtual BalanceExcel CreateExcel(
            string templatePath,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            Balance balance,
            string date)
        {
            return new BalanceExcel(templatePath, showingOption, convertingOption, balance, date);
        }

        private ActionResult Excel(BalanceViewModel model)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            const string FileName = "Balance";
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/Balance.xls");
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

            var balance = CreateExcel(
                templatePath,
                showingOption,
                convertingOption,
                model.MainData,
                model.Param.FromDate.ToString("MM - yyyy"));
            balance.Export();

            return new ExcelResult(balance.Workbook, FileName);
        }
    }
}