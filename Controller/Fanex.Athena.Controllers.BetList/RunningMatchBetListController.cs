// -----------------------------------------------------------------------
// <copyright file="RunningMatchBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Threading.Tasks;
    #region Using

    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Model.BetList.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class RunningMatchBetListController : BetListBaseController
    {
        public RunningMatchBetListController(
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
        public async Task<ActionResult> Index(BetListExcelMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return await ProcessDefaultRequest(
                metaModel, 
                "~/Views/Desktop/BetList/RunningBetListOfMatch/Main.cshtml", 
                "Lrf_BetList_SelRunningByBettypeMatch").ConfigureAwait(false);
        }

        protected override async Task<ActionResult> ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.Title = $"(LEAGUENAME) - {CacheRefAdapter.GetBettype(metaModel.BetType)} - {GetViewInCurrency(metaModel)}";

            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            metaModel.HiddenColumns = HiddenColumnIndexes;
            return await ProcessExportExcel(metaModel, "RunningBetList").ConfigureAwait(false);
        }
    }
}