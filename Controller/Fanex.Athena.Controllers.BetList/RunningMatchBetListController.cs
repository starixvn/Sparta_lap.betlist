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
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class RunningMatchBetListController : BetListBaseController
    {
        public RunningMatchBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(model, "~/Views/Desktop/BetList/RunningBetListOfMatch/Main.cshtml", null);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            const string UspName = "Lrf_BetList_SelRunningByBettypeMatch";

            var ticketResult = BetListService.GetBetListData(metaModel, UspName);

            var firstTicket = ticketResult.MainTicketList.FirstOrDefault();
            var leagueName = firstTicket != null ? CacheRefAdapter.GetLeagueName(firstTicket.LeagueId) : string.Empty;

            var title = $"{leagueName} - {CacheRefAdapter.GetBettype(metaModel.BetType)} - {GetViewInCurrency(metaModel)}";

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumnIndexes, title, "RunningBetList");
        }
    }
}