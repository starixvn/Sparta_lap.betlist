// -----------------------------------------------------------------------
// <copyright file="MicroGamingRngBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

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

    #endregion Using

    public class MicroGamingRngBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };

        public MicroGamingRngBetListController()
        {
        }

        public MicroGamingRngBetListController(
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
            return ProcessDefaultRequest(model, "~/Views/BetList/MicroGamingRngDetail/Main.cshtml", "Lrf_BetList_RNG_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = GetTitle(model.TransType);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var showingOption = new ShowingOption
            {
                GrandTotalVisible = false
            };

            var title = GetTitle(metaModel.TransType);
            var ticketResult = this.BetListService.GetBetListData(metaModel, uspKey);

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumns, title, "MicrogamingRNGDetail");
        }

        private static string GetTitle(long transType)
        {
            var title = ViewResources.Microgaming + " - " + ViewResources.ProductCS + " ";
            return GetTitleByTransType(title, transType);
        }
    }
}