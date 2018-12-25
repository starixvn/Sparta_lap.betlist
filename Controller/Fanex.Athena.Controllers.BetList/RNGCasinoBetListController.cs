// -----------------------------------------------------------------------
// <copyright file="RNGCasinoBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class RNGCasinoBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };

        public RNGCasinoBetListController(
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
            return ProcessDefaultRequest(CreateDefaultMetaModel(metaModel), "~/Views/Desktop/BetList/RngCasinoDetail/Main.cshtml", "Lrf_BetList_RNG_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = GetRNGCasinoBetListTitle(model.TransType);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var title = GetRNGCasinoBetListTitle(metaModel.TransType);
            var ticketResult = BetListService.GetBetListData(metaModel, UserCurrencyList, uspKey);
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumns, title, "RngCasinoDetail");
        }

        private static string GetRNGCasinoBetListTitle(long transType)
        {
            var title = ViewResources.ProductCS + " - ";
            return GetTitleByTransType(title, transType);
        }
    }
}