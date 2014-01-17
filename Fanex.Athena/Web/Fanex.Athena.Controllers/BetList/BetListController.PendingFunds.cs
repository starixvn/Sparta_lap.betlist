// -----------------------------------------------------------------------
// <copyright file="BetListController.PendingFunds.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
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
        /// Hidden column list (externalId,systemId,win-loss,licensee win-loss).
        /// </summary>
        private static readonly List<int> HiddenColList = new List<int> { 1, 2, 7, 8 };

        /// <summary>
        /// Action Get for pending funds bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult PendingFunds(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            metaModel.FromDate = DateTime.Today.AddYears(-10);
            metaModel.ToDate = DateTime.Today.AddYears(10);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
                }

                const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";

                return this.PendingFundsExcel(metaModel, UspKey);
            }

            return View("~/Views/BetList/PendingFunds/Main.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for pending funds bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="uspKey">The store procedure.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        public ActionResult PendingFundsExcel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = this._betListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var title = "Running " + ViewResources.BetList
                + string.Format(
                " - {0}({1}) - {2}",
                metaModel.ExternalId,
                metaModel.SystemId,
                metaModel.IsConvert ? ViewResources.Viewin + " " + metaModel.BaseCurrency.ToUpper() : ViewResources.Viewin + " " + metaModel.CurrencyName);

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, HiddenColList);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "LicenseeWinLossBetList");
        }
    }
}