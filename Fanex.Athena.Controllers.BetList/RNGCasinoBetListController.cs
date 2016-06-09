// -----------------------------------------------------------------------
// <copyright file="RNGCasinoBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    public class RNGCasinoBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = GetTitle(metaModel.TransType);
                return this.View("~/Views/BetList/RngCasinoDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return ExcelDetail(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private static string GetTitle(long transType)
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

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(new ShowingOption(), metaModel.LicenseeId == LicenseeId12Bet, hiddenColumns);
            var title = GetTitle(metaModel.TransType);
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RngCasinoDetail");
        }
    }
}