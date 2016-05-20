// -----------------------------------------------------------------------
// <copyright file="BetListController.CorrectScoreBetList.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for correct score.
        /// Used in TOTAL BETS.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CorrectScoreBetList()
        {
            var metaModel = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(metaModel);

            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = GetUserCurrencies(),
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this.ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/CorrectScoreBetList/Main.cshtml", model);
        }

        /// <summary>
        /// Action for correct score export excel.
        /// [Used in TOTAL BETS].
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult CorrectScoreBetList(BetListMetaModel metaModel)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var currencies = GetUserCurrencies();
            if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.Values.Contains(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_CorrectScore";
                metaModel = CreateDefaultMetaModel(metaModel);

                var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

                var showingOption = new ShowingOption
                {
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                };

                var title = ViewResources.CorrectScore + " - " + string.Format(
                    ViewResources.Viewin,
                    metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

                var excel = new Excel.BetList.BetListExcel(
                    showingOption, metaModel.LicenseeId == 1, HiddenColumnIndexes, true);

                var workbook = excel.Export(ticketResult, title);

                return new ExcelResult(workbook, "CorrectScoreBetList");
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }
    }
}