// -----------------------------------------------------------------------
// <copyright file="RunningMatchChoiceBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    public class RunningMatchChoiceBetListController : BetListBaseController
    {
        /// <summary>
        /// Action Get for Running bet list of match.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                    return Excel(model);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/NewForecastBetlist/Main.cshtml", model);
        }

        /// <summary>
        /// Export excel for running bet list of match.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Forecast_SelByMatch";
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

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
               string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, HiddenColumnIndexes, metaModel.IsShowSumStake);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RunningBetList");
        }
    }
}