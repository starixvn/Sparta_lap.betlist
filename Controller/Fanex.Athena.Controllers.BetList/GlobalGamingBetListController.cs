namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Web.ResultExtension;
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

            if (!model.IsExportExcel)
            {
                ViewBag.BetListTitle = _title;
                return View("~/Views/BetList/GlobalGamingDetail/Main.cshtml", model);
            }

            if (IsValidCurrency(model.CurrencyId))
            {
                return ExcelDetail(model);
            }

            return EmptyExcel();
        }

        private ActionResult ExcelDetail(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RNG_Detail";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var hiddenColumns = new List<int> { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };
            var excel = new BetListExcel(new ShowingOption { CustomerCommVisible = true }, SessionService.LicenseeSession.LicenseeId, hiddenColumns);
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);
            var workbook = excel.Export(ticketResult, _title);

            return new ExcelResult(workbook, "GlobalGamingDetail");
        }
    }
}