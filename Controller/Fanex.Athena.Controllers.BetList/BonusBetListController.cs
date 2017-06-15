namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base;
    using Excel.Base.Entities;
    using Excel.BetList;
    using Model.BetList.MetaModel;
    using Resources;
    using Web.ResultExtension;

    public class BonusBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            if (!metaModel.IsExportExcel)
            {
                return View("~/Views/BetList/BonusDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return ExcelDetail(metaModel);
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_LicenseeBonus_Detail";
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.LicenseeId = licenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };
            var showingOption = new ShowingOption { GrandTotalVisible = false, CustomerCommVisible = true };
            var excel = new BetListExcel(showingOption, licenseeId == LicenseeId12Bet, hiddenColumns);
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, ViewResources.TransactionDetail);

            return new ExcelResult(workbook, "BonusTransactionDetail");
        }
    }
}