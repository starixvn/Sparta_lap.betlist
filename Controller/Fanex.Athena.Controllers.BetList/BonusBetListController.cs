namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Model.BetList.MetaModel;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class BonusBetListController : BetListBaseController
    {
        public BonusBetListController(
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
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(CreateDefaultMetaModel(metaModel), "~/Views/Desktop/BetList/BonusDetail/Main.cshtml", "Lrf_BetList_LicenseeBonus_Detail");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            return ProcessExportExcel(
                new ShowingOption { GrandTotalVisible = false, CustomerCommVisible = true },
                metaModel,
                BetListService.GetBetListData(metaModel, uspKey),
                new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn },
                ViewResources.TransactionDetail,
                "BonusTransactionDetail");
        }
    }
}