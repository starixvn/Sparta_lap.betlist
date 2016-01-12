// -----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ResultExtension;

    public partial class BetListController
    {
        [HttpGet]
        public ActionResult RnGCasinoDetail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = GetBetListTitleForRngCasino(metaModel.TransType);
                return this.View("~/Views/BetList/RngCasinoDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return RngCasinoDetailExcel(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private ActionResult RngCasinoDetailExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(new ShowingOption(), metaModel.LicenseeId == LicenseeId12Bet, hiddenColumns);
            var title = GetBetListTitleForRngCasino(metaModel.TransType);
            var ticketResult = this._betListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RngCasinoDetail");
        }

        private string GetBetListTitleForRngCasino(long transType)
        {
            string title = ViewResources.ProductCS + " - " + ViewResources.BetList + " - " +
                ViewResources.DetailTickets + " - ";
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
