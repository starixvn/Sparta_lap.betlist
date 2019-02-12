namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;
    using Athena.BetList.Model.MetaModel;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    public class BonusBetListController : BetListBaseController
    {
        public BonusBetListController(
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
            return ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/BonusDetail/Main.cshtml", "Lrf_BetList_LicenseeBonus_Detail");
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.ShowingOption = new BetListShowingOption { GrandTotalVisible = false, CustomerCommVisible = metaModel.HasPlayerCom };
            metaModel.HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };
            metaModel.Title = ViewResources.TransactionDetail;
            return ProcessExportExcel(metaModel, "BonusTransactionDetail");
        }
    }
}