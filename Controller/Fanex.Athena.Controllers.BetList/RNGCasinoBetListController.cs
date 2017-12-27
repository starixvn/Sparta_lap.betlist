// -----------------------------------------------------------------------
// <copyright file="RNGCasinoBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
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
            var model = CreateDefaultMetaModel(metaModel);

            if (!model.IsExportExcel)
            {
                ViewBag.BetListTitle = GetTitle(model.TransType);
                return View("~/Views/BetList/RngCasinoDetail/Main.cshtml", model);
            }

            if (IsValidCurrency(model.CurrencyId))
            {
                return ExcelDetail(model);
            }

            return EmptyExcel();
        }

        private static string GetTitle(long transType)
        {
            var title = ViewResources.ProductCS + " - ";
            return GetTitleByTransType(title, transType);
        }

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(new ShowingOption(), SessionService.LicenseeSession.LicenseeId, hiddenColumns);
            var title = GetTitle(metaModel.TransType);
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RngCasinoDetail");
        }
    }
}