namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class GlobalGamingBetListController : BetListBaseController
    {
        private readonly string _title =
            ViewResources.ProductFW + " - " + ViewResources.BetList + " - " + ViewResources.DetailTickets;

        public GlobalGamingBetListController()
        {
        }

        public GlobalGamingBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(
            languageService,
            sessionService,
            appSetting,
            clientViewHelper,
            authorizationService,
            productService,
            betListService,
            balanceUpDataAccess,
            cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(model, "~/Views/BetList/GlobalGamingDetail/Main.cshtml", "Lrf_BetList_RNG_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = _title;
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            return ProcessExportExcel(new ShowingOption { CustomerCommVisible = true }, metaModel, ticketResult, hiddenColumns, _title, "GlobalGamingDetail");
        }
    }
}