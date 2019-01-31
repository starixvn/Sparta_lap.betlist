// -----------------------------------------------------------------------
// <copyright file="AllBetBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class AllBetBetListController : BetListBaseController
    {
        public AllBetBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Detail(BetListExcelMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/AGCasinoDetail/Main.cshtml", "Lrf_BetList_LC_Detail");
        }

        protected override void AssignViewBag(BetListMetaModel model)
        {
            ViewBag.BetListTitle = GetTitle(model.Date);
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.ShowingOption = new BetListShowingOption
            {
                GrandTotalVisible = false,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            metaModel.HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn, LicenseeWLColumn };
            metaModel.Title = GetTitle(metaModel.Date);

            return ProcessExportExcel(metaModel, "AllBetDetail");
        }

        private static string GetTitle(DateTime date)
        {
            return ViewResources.ProductAB + " - " + ViewResources.DetailedBetlist + " - " + date.ToString("MM/dd/yyyy");
        }
    }
}