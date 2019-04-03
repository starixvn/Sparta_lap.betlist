// -----------------------------------------------------------------------
// <copyright file="RunningBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    /// <summary>
    /// RunningBetList contains bet list:.
    /// Mix Parlay              - bet type: 9.
    /// Clean Sheet             - bet type: 13.
    /// Double Chance           - bet type: 24.
    /// B/O/N team to score     - bet type: 26.
    /// To win to nil           - bet type: 27.
    /// 3-Way Handicap          - bet type: 28.
    /// 1H 3-Way Handicap       - bet type: 453.
    /// </summary>
    public class RunningBetListController : BetListBaseController
    {
        public RunningBetListController(
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
        [FunctionAuthorization(PermissionKeys.TotalBetCleanSheet)]
        public async Task<ActionResult> CleanSheet(BetListExcelMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.CleanSheet;
            ViewBag.ReportTitle = ViewResources.CleanSheet;
            ViewBag.UserGuideName = "TotalBetsCleanSheet";
            return await ProcessRequest(metaModel).ConfigureAwait(false);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetToWinToNil)]
        public async Task<ActionResult> ToWinToNil(BetListExcelMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ToWinToNil;
            ViewBag.ReportTitle = ViewResources.ToWinToNil;
            ViewBag.UserGuideName = "TotalBetsToWinToNil";
            return await ProcessRequest(metaModel).ConfigureAwait(false);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBet3WayHandicap)]
        public async Task<ActionResult> ThreeWayHandicap(BetListExcelMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ThreeWayHandicap;
            metaModel.MultipleBettype = $"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}";
            ViewBag.ReportTitle = ViewResources.lbl3WayHandicap;
            ViewBag.UserGuideName = "TotalBets3WayHandicap";
            return await ProcessRequest(metaModel, "Lrf_BetList_SelRunningByBettypeList").ConfigureAwait(false);
        }

        private async Task<ActionResult> ProcessRequest(BetListExcelMetaModel metaModel, string uspKey = "Lrf_BetList_SelRunningByBettype")
        {
            var userSession = SessionService.UserSession;
            CreateDefaultMetaModel(metaModel);
            var currencies = userSession.CurrencyList;

            if (metaModel.IsExportExcel)
            {
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
                {
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                    return await Excel(metaModel, uspKey).ConfigureAwait(false);
                }

                return EmptyExcel();
            }

            SetConvetedUSDCurrencyName(metaModel);

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = currencies,
                ProductAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Desktop/BetList/RunningBetList/Main.cshtml", model);
        }

        private async Task<ActionResult> Excel(BetListExcelMetaModel metaModel, string uspKey)
        {
            metaModel.UspKey = uspKey;

            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var baseCurrency = GetViewInCurrency(metaModel);
            metaModel.Title = $"{CacheRefAdapter.GetBettype(metaModel.BetType)} - {baseCurrency}";

            metaModel.IsShowSumStake = true;
            metaModel.HiddenColumns = HiddenColumnIndexes;
            return await ProcessExportExcel(metaModel, "RunningBetList").ConfigureAwait(false);
        }
    }
}