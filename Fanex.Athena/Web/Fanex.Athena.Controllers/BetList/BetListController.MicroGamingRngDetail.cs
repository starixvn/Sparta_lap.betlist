// -----------------------------------------------------------------------
// <copyright file="BetListController.MicroGamingRngDetail.cs" company="Nexcel">
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
        private const int MicroGamingRngTransTypeYesterday = 0;

        private const int MicroGamingRngTransTypeTodayRunning = 3;

        [HttpGet]
        public ActionResult MicroGamingRngDetail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = GetBetListTitle(metaModel.TransType);
                return this.View("~/Views/BetList/MicroGamingRngDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return MicroGamingRngDetailExcel(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private ActionResult MicroGamingRngDetailExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";

            var showingOption = new ShowingOption()
            {
                GrandTotalVisible = false
            };

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == LicenseeId12Bet, hiddenColumns);
            var title = GetBetListTitle(metaModel.TransType);
            var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "MicrogamingRNGDetail");
        }

        private string GetBetListTitle(long transType)
        {
            string title = ViewResources.Microgaming + " - " + ViewResources.ProductCS + " " +
                ViewResources.BetList + " - " + ViewResources.DetailTickets + " - ";
            switch (transType)
            {
                case MicroGamingRngTransTypeYesterday:
                    title += ViewResources.Yesterday;
                    break;

                case MicroGamingRngTransTypeTodayRunning:
                    title += ViewResources.Today + " - " + ViewResources.Running;
                    break;

                default:
                    title += ViewResources.Today + " - " + ViewResources.Settled;
                    break;
            }

            return title;
        }
    }
}