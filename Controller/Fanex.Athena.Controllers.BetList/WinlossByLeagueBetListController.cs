// -----------------------------------------------------------------------
// <copyright file="WinlossByLeagueBetListController.cs" company="Nexdev">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportWinLossByLeague)]
    public class WinlossByLeagueBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

        public WinlossByLeagueBetListController(
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
        public ActionResult Index(WinLossByLeagueBetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(model, "~/Views/Desktop/BetList/WinlossByLeague/Main.cshtml", "Lrf_BetList_WinlostByLeague_Mem");
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, UserCurrencyList, uspKey);

            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var title = string.Format(
                "{0} - {1} → {2} - {3}{4} - {5}",
                ViewResources.CustomerBetList,
                metaModel.FromDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ToDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                viewinCurrency);
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            return ProcessExportExcel(showingOption, metaModel, ticketResult, HiddenColumns, title, "WinLossByLeagueBetList");
        }
    }
}