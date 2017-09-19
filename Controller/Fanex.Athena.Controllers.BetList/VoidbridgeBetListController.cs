namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    public class VoidbridgeBetListController : BetListBaseController
    {
        private readonly string _title =
            ViewResources.ProductVB + " - " + ViewResources.BetList + " - " + ViewResources.DetailTickets;

        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = _title;
                return this.View("~/Views/BetList/VoidbridgeDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return ExcelDetail(metaModel);
            }

            return this.EmptyExcel();
        }

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(new ShowingOption(), SessionService.LicenseeSession.LicenseeId == LicenseeId12Bet, hiddenColumns);
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, _title);

            return new ExcelResult(workbook, "MacauGamesDetail");
        }
    }
}