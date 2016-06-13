// -----------------------------------------------------------------------
// <copyright file="MicroGamingRngBetListController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    public class MicroGamingRngBetListController : BetListBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (!metaModel.IsExportExcel)
            {
                ViewBag.BetListTitle = GetTitle(metaModel.TransType);
                return this.View("~/Views/BetList/MicroGamingRngDetail/Main.cshtml", metaModel);
            }

            if (IsValidCurrency(metaModel.CurrencyId))
            {
                return ExcelDetail(metaModel);
            }

            return this.File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        private static string GetTitle(long transType)
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

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";

            var showingOption = new ShowingOption()
            {
                GrandTotalVisible = false
            };

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == LicenseeId12Bet, hiddenColumns);
            var title = GetTitle(metaModel.TransType);
            var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "MicrogamingRNGDetail");
        }
    }
}