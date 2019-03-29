namespace Fanex.Athena.Controllers.BetList
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetList.MetaModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { BetEffectiveColumn };

        public LiveReportBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IBetListApiClient betListApiClient)
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter, betListApiClient)
        {
        }

        [HttpGet]
        public Task<ActionResult> Match(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossByMatch",
                "~/Views/Desktop/BetList/LiveReport/Match.cshtml");
        }

        [HttpGet]
        public Task<ActionResult> MixParlay(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossParlay",
                "~/Views/Desktop/BetList/LiveReport/MixParlay.cshtml");
        }

        [HttpGet]
        public Task<ActionResult> Outright(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossOutright",
                "~/Views/Desktop/BetList/LiveReport/Outright.cshtml",
                ViewResources.Outright);
        }

        [HttpGet]
        public Task<ActionResult> NumberGame(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossNumberGame",
                "~/Views/Desktop/BetList/LiveReport/NumberGame.cshtml",
                ViewResources.ProductNG);
        }

        [HttpGet]
        public async Task<ActionResult> VirtualSports(LiveReportBetListMetaModel metaModel)
        {
            var sportName = CacheRefAdapter.GetSportName(metaModel.SportType);
            return await ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossVHRacing",
                "~/Views/Desktop/BetList/LiveReport/VirtualSports.cshtml",
                sportName,
                sportName).ConfigureAwait(false);
        }

        private string GetMatchName(LiveReportBetListMetaModel model)
        {
            return model.HomeId > 0 && model.AwayId > 0 ?
                CacheRefAdapter.GetTeamName(model.HomeId) + " vs " + CacheRefAdapter.GetTeamName(model.AwayId) + " " :
                string.Empty;
        }

        private async Task<ActionResult> ProcessRequest(
            LiveReportBetListMetaModel metaModel,
            string uspKey,
            string viewPath,
            string caption = null,
            string sportName = null)
        {
            await CreateDefaultMetaModel(metaModel).ConfigureAwait(false);
            if (!metaModel.IsExportExcel)
            {
                ViewBag.SportName = sportName;
                return View(viewPath, metaModel);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
            {
                var title = GetExcelTitle(metaModel, caption);
                return await Excel(metaModel, uspKey, title).ConfigureAwait(false);
            }

            return EmptyExcel();
        }

        private async Task<ActionResult> Excel(LiveReportBetListMetaModel metaModel, string uspKey, string title)
        {
            // Sum value by page
            metaModel.UspKey = uspKey;
            metaModel.Title = title;
            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            metaModel.StartIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            metaModel.HiddenColumns = HiddenColumns;
            return await ProcessExportExcel(metaModel, "LiveReportBetList", "/BetList/ExportExcelLiveReport").ConfigureAwait(false);
        }

        private string GetExcelTitle(LiveReportBetListMetaModel metaModel, string caption = null)
        {
            var betlistCaption = string.IsNullOrEmpty(caption) ?
                   GetMatchName(metaModel) + CacheRefAdapter.GetBettype(metaModel.BetType) :
                   caption;
            var viewInCaption = GetViewInCurrency(metaModel);
            return ViewResources.BetList
                + string.Format(
                " - {0} - {1} --> {2} - {3}",
                betlistCaption,
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                viewInCaption);
        }
    }
}