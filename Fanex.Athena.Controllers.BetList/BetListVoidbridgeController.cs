namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    public class BetListVoidbridgeController : BetListBaseController
    {
        private readonly string _title =
            ViewResources.ProductVB + " - " + ViewResources.BetList + " - " + ViewResources.DetailTickets;

        [HttpGet]
        public ActionResult VoidbridgeDetail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = _title;
                return this.View("~/Views/BetList/VoidbridgeDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return VoidbridgeDetailExcel(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private ActionResult VoidbridgeDetailExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(new ShowingOption(), metaModel.LicenseeId == LicenseeId12Bet, hiddenColumns);
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, _title);

            return new ExcelResult(workbook, "VoidbridgeDetail");
        }
    }
}