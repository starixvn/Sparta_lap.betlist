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
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class RunningMatchBetListController : BetListBaseController
    {
        public RunningMatchBetListController()
        {
        }

        public RunningMatchBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(
            languageService,
            sessionService,
            appSetting,
            clientViewHelper,
            authorizationService,
            productService,
            betListService,
            balanceUpDataAccess,
            cacheRefAdapter)
        {
        }

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
                leagueName = CacheRefAdapter.GetLeagueName(firstTicket.LeagueId);
            }

            var title = leagueName + " - " + string.Format(
                "{0} - {1}",
                CacheRefAdapter.GetBettype(metaModel.BetType),
                GetViewInCurrency(metaModel));

            return ProcessExportExcel(metaModel, ticketResult, HiddenColumnIndexes, title, "RunningBetList");
        }
    }
}