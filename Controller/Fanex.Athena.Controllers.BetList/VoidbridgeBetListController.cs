namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    public class VoidbridgeBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };

        private readonly string _title =
            ViewResources.ProductVB + " - " + ViewResources.BetList + " - " + ViewResources.DetailTickets;

        public VoidbridgeBetListController(
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
        public ActionResult Detail(BetListExcelMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/VoidbridgeDetail/Main.cshtml", "Lrf_BetList_RNG_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = _title;
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            metaModel.Title = _title;
            metaModel.HiddenColumns = HiddenColumns;
            return ProcessExportExcel(metaModel, "MacauGamesDetail");
        }
    }
}