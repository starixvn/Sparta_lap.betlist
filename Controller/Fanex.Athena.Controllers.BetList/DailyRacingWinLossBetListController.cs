// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportDailyRacingWinLoss)]
    public class DailyRacingWinLossBetListController : BetListBaseController
    {
        public DailyRacingWinLossBetListController()
        {
        }

        public DailyRacingWinLossBetListController(
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
        public ActionResult Index(BetListMetaModel metaModel)
        {
            return ProcessDefaultRequest(metaModel, "~/Views/BetList/DailyRacingWl/Main.cshtml");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinlostRacing";
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

            var currencyTitle = string.Format(
                ViewResources.Viewin,
                metaModel.CurrencyId == BaseCurrencyType.All || metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName);
            var title = ViewResources.DailyRacingWinLoss + " " + ViewResources.BetList
                + string.Format(
                " - {0} - {1}",
                metaModel.Date.ToString("MMM dd, yyyy"),
                currencyTitle);

            var hiddenColumns = new List<int> { BetEffectiveColumn };

            return ProcessExportExcel(metaModel, ticketResult, hiddenColumns, title, "DailyRacingWinLossBetList");
        }
    }
}