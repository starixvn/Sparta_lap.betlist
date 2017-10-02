// -----------------------------------------------------------------------
// <copyright file="RunningBettypesOfMatchBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ResultExtension;
    using Fanex.Common;
    using Resources;

    #endregion Using

    public class RunningBettypesOfMatchBetListController : BetListBaseController
    {
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

            return View("~/Views/BetList/RunningBettypesOfMatchBetList/Main.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelRunningByMultipleBettypeMatch";
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

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

            var bettype = metaModel.BetType;
            if (metaModel.BetType == 0 && !string.IsNullOrWhiteSpace(metaModel.MultipleBettype))
            {
                bettype = Converter.ToInt(metaModel.MultipleBettype.Split(',')[0]);
            }

            var title = leagueName + " - " + string.Format(
                "{0} - {1}",
                CacheRef.GetBettype(bettype),
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, HiddenColumnIndexes, true);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RunningBetList");
        }
    }
}