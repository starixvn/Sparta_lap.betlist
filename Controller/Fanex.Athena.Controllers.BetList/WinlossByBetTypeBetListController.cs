namespace Fanex.Athena.Controllers.BetList
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetList.MetaModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [FunctionAuthorization(PermissionKeys.ReportWinLossByBetType)]
    public class WinlossByBetTypeBetListController : BetListBaseController
    {
        public WinlossByBetTypeBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IBetListApiClient betListApiClient)
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter, betListApiClient)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index(BetListExcelMetaModel metaModel)
        {
            await CreateDefaultMetaModel(metaModel).ConfigureAwait(false);
            return await ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/WinlossByBetType/Main.cshtml", "Lrf_BetList_SelWinLossByBetType").ConfigureAwait(false);
        }

        protected override bool IsValidExportExcel(BetListMetaModel model)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return model.CurrencyId != BaseCurrencyType.All &&
                currencyList.ContainsKey(model.CurrencyId) &&
                model.FromDate.Date == model.ToDate.Date;
        }

        protected override async Task<ActionResult> ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;

            // determine currency viewed in
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            metaModel.Title = string.Format(
                "{0} - {1} - {2} → {3} - {4}",
                ViewResources.BetList,
                CacheRefAdapter.GetBettype(metaModel.BetType),
                metaModel.FromDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ToDate.ToString(FormatKey.ShortDateFormat),
                viewinCurrency);
            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            metaModel.HiddenColumns = new[] { BetEffectiveColumn };
            return await ProcessExportExcel(metaModel, "WinLossByBetTypeBetList").ConfigureAwait(false);
        }
    }
}