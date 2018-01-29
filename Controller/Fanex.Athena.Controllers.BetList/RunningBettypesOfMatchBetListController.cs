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
            return ProcessDefaultRequest(metaModel, "~/Views/BetList/RunningBettypesOfMatchBetList/Main.cshtml");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelRunningByMultipleBettypeMatch";
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);
            
            var leagueName = string.Empty;
            var firstTicket = ticketResult.MainTicketList.FirstOrDefault();
            if (ticketResult.MainTicketList.Any() && firstTicket != null)
            {
                leagueName = CacheRefAdapter.GetLeagueName(firstTicket.LeagueId);
            }

            var bettype = metaModel.BetType;
            if (metaModel.BetType == 0 && !string.IsNullOrWhiteSpace(metaModel.MultipleBettype))
            {
                bettype = Converter.ToInt(metaModel.MultipleBettype.Split(',')[0]);
            }

            var title = leagueName + " - " + string.Format(
                "{0} - {1}",
                CacheRefAdapter.GetBettype(bettype),
                GetViewInCurrency(metaModel));
            return ProcessExportExcel(metaModel, ticketResult, HiddenColumnIndexes, title, "RunningBetList");
        }
    }
}