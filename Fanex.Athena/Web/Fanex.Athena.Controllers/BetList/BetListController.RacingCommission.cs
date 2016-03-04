// -----------------------------------------------------------------------
// <copyright file="BetListController.CustomerWl.cs" company="Nexcel">
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
        /// Action Get for Customer win lost bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult RacingCommission(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return RacingCommissionExcel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/RacingCommission/Main.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for customer win lost.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        public ActionResult RacingCommissionExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinLossRacingByDate_Mem";

            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

            var title = ViewResources.Customer + " " + ViewResources.BetList + string.Format(
                " - {0} --> {1} - {2}({3}) - {4}",
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                metaModel.ExternalId,
                metaModel.SystemId,
                currencyTitle);

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "CustomerWinLossBetList");
        }
    }
}