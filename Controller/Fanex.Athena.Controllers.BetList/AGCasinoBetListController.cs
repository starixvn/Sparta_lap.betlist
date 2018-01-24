// -----------------------------------------------------------------------
// <copyright file="AGCasinoBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    public class AGCasinoBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);

            if (!model.IsExportExcel)
            {
                ViewBag.BetListTitle = GetTitle(model.TransType);
                return View("~/Views/BetList/AGCasinoDetail/Main.cshtml", model);
            }

            if (IsValidCurrency(model.CurrencyId))
            {
                return ExcelDetail(model);
            }

            return EmptyExcel();
        }

        private static string GetTitle(long transType)
        {
            return GetTitleByTransType(ViewResources.ProductAGC + " - ", transType);
        }

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_LC_Detail";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var showingOption = new ShowingOption
            {
                GrandTotalVisible = false
            };

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(showingOption, SessionService.LicenseeSession.LicenseeId, hiddenColumns);
            var title = GetTitle(metaModel.TransType);
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "AGCasinoDetail");
        }
    }
}