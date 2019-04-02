// -----------------------------------------------------------------------
// <copyright file="MicroGamingRngBetListController.cs" company="">
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
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class MicroGamingRngBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };

        public MicroGamingRngBetListController(
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
        public async Task<ActionResult> Detail(BetListExcelMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return await ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/MicroGamingRngDetail/Main.cshtml", "Lrf_BetList_RNG_Detail").ConfigureAwait(false);
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = GetTitle(model.TransType);
        }

        protected override async Task<ActionResult> ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.ShowingOption = new BetListShowingOption
            {
                GrandTotalVisible = false,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            metaModel.Title = GetTitle(metaModel.TransType);
            metaModel.HiddenColumns = HiddenColumns;
            return await ProcessExportExcel(metaModel, "MicrogamingRNGDetail").ConfigureAwait(false);
        }

        private static string GetTitle(long transType)
        {
            var title = ViewResources.Microgaming + " - " + ViewResources.ProductCS + " ";
            return GetTitleByTransType(title, transType);
        }
    }
}