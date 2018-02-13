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
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Common;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class RunningBettypesOfMatchBetListController : BetListBaseController
    {
        public RunningBettypesOfMatchBetListController()
        {
        }

        public RunningBettypesOfMatchBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, clientViewHelper, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(model, "~/Views/BetList/RunningBettypesOfMatchBetList/Main.cshtml", "Lrf_BetList_SelRunningByMultipleBettypeMatch");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            var firstTicket = ticketResult.MainTicketList.FirstOrDefault();
            var leagueName = firstTicket != null ? CacheRefAdapter.GetLeagueName(firstTicket.LeagueId) : string.Empty;

            var bettype = metaModel.BetType == 0 && !string.IsNullOrWhiteSpace(metaModel.MultipleBettype) 
                ? Converter.ToInt(metaModel.MultipleBettype.Split(',')[0])
                : metaModel.BetType;

            var title = $"{leagueName} - {CacheRefAdapter.GetBettype(bettype)} - {GetViewInCurrency(metaModel)}";
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumnIndexes, title, "RunningBetList");
        }
    }
}