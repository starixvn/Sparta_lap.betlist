// -----------------------------------------------------------------------
// <copyright file="RunningMatchChoiceBetListController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    public class RunningMatchChoiceBetListController : BetListBaseController
    {
        /// <summary>
        /// Action Get for Running bet list of match.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return Excel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/NewForecastBetlist/Main.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for running bet list of match.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        private ActionResult Excel(BetListMetaModel metaModel)
        {
            string uspKey = "Lrf_BetList_Forecast_SelByMatch";
            var ticketResult = this.BetListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            var leagueName = string.Empty;
            var firstTicket = ticketResult.MainTicketList.FirstOrDefault();
            if (ticketResult.MainTicketList.Any() && firstTicket != null)
            {
                leagueName = CacheRef.GetLeagueName(firstTicket.LeagueId);
            }

            var title = leagueName + " - " + string.Format(
                "{0} - {1}",
                CacheRef.GetBettype(metaModel.BetType),
               string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, HiddenColumnIndexes, metaModel.IsShowSumStake);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RunningBetList");
        }
    }
}