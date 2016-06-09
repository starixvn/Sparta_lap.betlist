// -----------------------------------------------------------------------
// <copyright file="ReportController.Balance.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     Credit Balance controller.
    /// </summary>
    public class BalanceReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var metaModel = new BalanceMetaModel() { LicenseeId = SessionService.LicenseeSession.LicenseeId };
            SetConvetedUSDCurrencyName(metaModel);
            return Index(metaModel);
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "None")]
        [FunctionAuthorization]
        public ActionResult Index(BalanceMetaModel metaModel)
        {
            AddNotificationMessageToViewBag();

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            metaModel.MoveBetsDay = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            var creditBalanceService = new CreditBalanceService();
            var reportData = creditBalanceService.GetReportData(metaModel);

            var viewData = new BalanceViewModel
            {
                MainData = reportData,
                Param = metaModel,
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                CurrencyList = GetUserCurrencies(),
                LicenseeTypeList = licenseeSession.LicenseeType,
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
                RBVisible = productAccess.IsAllowRB,
                BAVisible = productAccess.IsAllowBA,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB,
                LCVisible = productAccess.IsAllowLC,
                VSVisible = productAccess.IsAllowVS,
                CSVisible = productAccess.IsAllowCS,
                BGVisible = productAccess.IsAllowBG,
                KNVisible = productAccess.IsAllowKN,
                BTVisible = productAccess.IsAllowBT,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var balance = new BalanceExcel(
                templatePath,
                FileName,
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