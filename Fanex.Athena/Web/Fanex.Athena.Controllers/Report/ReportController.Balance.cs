// -----------------------------------------------------------------------
// <copyright file="ReportController.Balance.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     Credit Balance controller.
    /// </summary>
    public partial class ReportController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Balance()
        {
            var metaModel = new BalanceMetaModel() { LicenseeId = SessionService.LicenseeSession.LicenseeId };
            SetConvetedUSDCurrencyName(metaModel);
            return Balance(metaModel);
        }

        /// <summary>
        /// Balance report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "None")]
        [FunctionAuthorization]
        public ActionResult Balance(BalanceMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            var creditBalanceService = new CreditBalanceService();            
            var reportData = creditBalanceService.GetReportData(metaModel);

            var viewData = new BalanceViewModel
                {
                    MainData = reportData,
                    Param = metaModel,
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    CurrencyList = userSession.CurrencyList,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    ProductAccess = productAccess,
                };

            if (metaModel.IsExportExcel)
            {
                return BalanceExcel(viewData);
            }

            ViewBag.BANotes = productAccess.CanUseProduct(Product.BA);
            ViewBag.RacingNotes = productAccess.IsAllowRB;

            return View("~/Views/Report/Balance/Main.cshtml", viewData);
        }

        #region Export Excel

        /// <summary>
        /// Balance Excel report.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>The excel file.</returns>
        private ActionResult BalanceExcel(BalanceViewModel model)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            const string FileName = "Balance";
            var templatePath = Server.MapPath("~/App_Data/Template/Reports/Balance.xls");
            var productAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);

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
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
                };

            var balance = new BalanceExcel(
                templatePath,
                FileName,
                showingOption,
                convertingOption,
                model.MainData,
                model.Param.FromDate.ToString("MM - yyyy")) { ProductOptions = model.Param.ProductOptions };
            balance.Export();

            return new ExcelResult(balance.Workbook, FileName);
        }

        #endregion Export Excel
    }
}