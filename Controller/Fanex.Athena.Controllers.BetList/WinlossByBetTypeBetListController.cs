namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [FunctionAuthorization(PermissionKeys.ReportWinLossByBetType)]
    public class WinlossByBetTypeBetListController : BetListBaseController
    {
        public WinlossByBetTypeBetListController()
        {
        }

        public WinlossByBetTypeBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(
            languageService,
            sessionService,
            appSetting,
            clientViewHelper,
            productService,
            betListService,
            balanceUpDataAccess,
            cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(model, "~/Views/BetList/WinlossByBetType/Main.cshtml", "Lrf_BetList_SelWinLossByBetType");
        }

        protected override bool IsValidExportExcel(BetListMetaModel model)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return model.CurrencyId != BaseCurrencyType.All &&
                currencyList.ContainsKey(model.CurrencyId) &&
                model.FromDate.Date == model.ToDate.Date;
        }

        protected override ActionResult ExportExcel(BetListMetaModel metaModel, string uspKey)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            // determine currency viewed in
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var title = string.Format(
                "{0} - {1} - {2} → {3} - {4}",
                ViewResources.BetList,
                CacheRefAdapter.GetBettype(metaModel.BetType),
                metaModel.FromDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ToDate.ToString(FormatKey.ShortDateFormat),
                viewinCurrency);

            return ProcessExportExcel(new ShowingOption(), metaModel, ticketResult, new List<int> { BetEffectiveColumn }, title, "WinLossByBetTypeBetList");
        }
    }
}