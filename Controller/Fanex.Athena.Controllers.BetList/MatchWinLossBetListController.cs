// -----------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { BetEffectiveColumn };

        public MatchWinLossBetListController(
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
        public ActionResult MatchBetType(BetListMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, "~/Views/BetList/MatchWl/MatchBetType.cshtml");
        }

        [HttpGet]
        public ActionResult MixParlay(BetListMetaModel metaModel)
        {
            return ProcessRequest(metaModel, "Lrf_BetList_MatchWL_Parlay", "~/Views/BetList/MatchWl/MixParlay.cshtml");
        }

        [HttpGet]
        public ActionResult Outright(BetListMetaModel metaModel)
        {
            return ProcessRequest(metaModel, "Lrf_BetList_MatchWL_Outright", "~/Views/BetList/MatchWl/Outright.cshtml");
        }

        private ActionResult ProcessRequest(BetListMetaModel metaModel, string uspKey, string viewPath)
        {
            var model = CreateDefaultMetaModel(metaModel);
            model.IsServerPaging = true;
            return ProcessDefaultRequest(model, viewPath, uspKey);
        }

        protected override bool IsValidExportExcel(BetListMetaModel model)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;

            var uspName = string.IsNullOrEmpty(uspKey) ? "Lrf_BetList_MatchWL_Bettype" : uspKey;
            var ticketResult = BetListService.GetBetListData(metaModel, uspName);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                  ? CacheRefAdapter.GetTeamName(metaModel.HomeId) + " vs " + CacheRefAdapter.GetTeamName(metaModel.AwayId) + " - "
                  : string.Empty;
            var currencyTitle = GetViewInCurrency(metaModel);

            var title = ViewResources.BetList
                + string.Format(
                " - {0}{1} - {2}",
                matchName + (metaModel.BetType > 0 ? CacheRefAdapter.GetBettype(metaModel.BetType) + " - " : string.Empty),
                metaModel.FromDate.ToShortDateString(),
                currencyTitle);

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumns, title, "MatchWinLossBetList");
        }
    }
}