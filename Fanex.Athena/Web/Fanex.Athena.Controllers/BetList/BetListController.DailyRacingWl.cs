// -----------------------------------------------------------------------
// <copyright file="BetListController.DailyRacingWl.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ResultExtension;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for daily racing win lost.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult DailyRacingWL(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return DailyRacingWLPost(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/DailyRacingWl/Main.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for daily racing win lost.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        public ActionResult DailyRacingWLPost(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinlostRacing";
            var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.CurrencyId == BaseCurrencyType.All || metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
            var title = ViewResources.DailyRacingWinLoss + " " + ViewResources.BetList
                + string.Format(
                " - {0} - {1}",
                metaModel.Date.ToString("MMM dd, yyyy"),
                currencyTitle);

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, new List<int> { BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "DailyRacingWinLossBetList");
        }
    }
}