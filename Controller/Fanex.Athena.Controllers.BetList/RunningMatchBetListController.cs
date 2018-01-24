// -----------------------------------------------------------------------
// <copyright file="RunningMatchBetListController.cs" company="">
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
    using Resources;

    #endregion Using

    public class RunningMatchBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(metaModel, "~/Views/BetList/RunningBetListOfMatch/Main.cshtml");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel)
        {
            var uspKey = IsRacing(metaModel.BetType)
                                              ? "Lrf_BetList_SelAllByDateHR"
                                              : "Lrf_BetList_SelRunningByBettypeMatch";
            var ticketResult = this.BetListService.GetBetListData(metaModel, uspKey);
            
            var leagueName = string.Empty;
            var firstTicket = ticketResult.MainTicketList.FirstOrDefault();
            if (ticketResult.MainTicketList.Any() && firstTicket != null)
            {
                leagueName = CacheRef.GetLeagueName(firstTicket.LeagueId);
            }

            var title = leagueName + " - " + string.Format(
                "{0} - {1}",
                CacheRef.GetBettype(metaModel.BetType),
                GetViewInCurrency(metaModel));
            
            return ProcessExportExcel(metaModel, ticketResult, HiddenColumnIndexes, title, "RunningBetList");
        }
    }
}