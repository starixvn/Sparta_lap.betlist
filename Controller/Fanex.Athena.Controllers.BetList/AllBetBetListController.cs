// -----------------------------------------------------------------------
// <copyright file="AllBetBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
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

    public class AllBetBetListController : BetListBaseController
    {
        public AllBetBetListController(
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
            var model = CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(model, "~/Views/BetList/AGCasinoDetail/Main.cshtml", "Lrf_BetList_LC_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = GetTitle(model.Date);
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var showingOption = new ShowingOption
            {
                GrandTotalVisible = false,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };

            var title = GetTitle(metaModel.Date);
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            return ProcessExportExcel(showingOption, metaModel, ticketResult, hiddenColumns, title, "AllBetDetail");
        }

        private static string GetTitle(DateTime date)
        {
            return ViewResources.ProductAB + " - " + ViewResources.DetailedBetlist + " - " + date.ToString("MM/dd/yyyy");
        }
    }
}