// -----------------------------------------------------------------------
// <copyright file="RunningBettypesOfMatchBetListController.cs" company="">
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
    using Fanex.Common;
    using Model.BetList.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class RunningBettypesOfMatchBetListController : BetListBaseController
    {
        public RunningBettypesOfMatchBetListController(
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
            await CreateDefaultMetaModel(metaModel).ConfigureAwait(false);
            return await ProcessDefaultRequest(
                metaModel, 
                "~/Views/Desktop/BetList/RunningBettypesOfMatchBetList/Main.cshtml", 
                "Lrf_BetList_SelRunningByMultipleBettypeMatch").ConfigureAwait(false);
        }

        protected override async Task<ActionResult> ExportExcel(BetListExcelMetaModel metaModel)
        {
            var bettype = metaModel.BetType == 0 && !string.IsNullOrWhiteSpace(metaModel.MultipleBettype)
                ? Converter.ToInt(metaModel.MultipleBettype.Split(',')[0])
                : metaModel.BetType;

            metaModel.Title = $"(LEAGUENAME) - {CacheRefAdapter.GetBettype(bettype)} - {GetViewInCurrency(metaModel)}";
            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            metaModel.HiddenColumns = HiddenColumnIndexes;
            return await ProcessExportExcel(metaModel, "RunningBetList").ConfigureAwait(false);
        }
    }
}