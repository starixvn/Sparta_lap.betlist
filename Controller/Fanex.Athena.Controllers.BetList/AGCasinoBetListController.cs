// -----------------------------------------------------------------------
// <copyright file="AGCasinoBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

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

    #endregion Using

    public class AGCasinoBetListController : BetListBaseController
    {
        public AGCasinoBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, clientViewHelper, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Detail(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(CreateDefaultMetaModel(metaModel), "~/Views/BetList/AGCasinoDetail/Main.cshtml", "Lrf_BetList_LC_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = GetAGCasinoBetListTitle(model.TransType);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var showingOption = new ShowingOption
            {
                GrandTotalVisible = false,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            var title = GetAGCasinoBetListTitle(metaModel.TransType);
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            return ProcessExportExcel(showingOption, metaModel, ticketResult, hiddenColumns, title, "AGCasinoDetail");
        }

        private static string GetAGCasinoBetListTitle(long transType)
        {
            return GetTitleByTransType(ViewResources.ProductAGC + " - ", transType);
        }
    }
}