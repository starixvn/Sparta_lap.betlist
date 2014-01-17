// -----------------------------------------------------------------------
// <copyright file="BetListController.CorrectScoreBetList.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.BetList;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

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
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
            var licenseeSession = this.SessionService.LicenseeSession;
            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = SessionService.UserSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this._productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language),
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
            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == 0 || currencyList.Values.Contains(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_CorrectScore";
                metaModel = CreateDefaultMetaModel(metaModel);

                var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);

                var showingOption = new ShowingOption
                {
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                };

                var title = ViewResources.CorrectScore + " - " + ViewResources.Viewin + " " +
                    (metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

                var hiddenColumnIdx = new List<int> { 7, 8 };

                var excel = new Excel.BetList.BetListExcel(
                    showingOption, metaModel.LicenseeId == 1, hiddenColumnIdx);

                var workbook = excel.Export(ticketResult, title);

                return new ExcelResult(workbook, "CorrectScoreBetList");
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }
    }
}