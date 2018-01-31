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
            var model = CreateDefaultMetaModel(metaModel);
            if (!model.IsExportExcel)
            {
                return View("~/Views/BetList/WinlossByBetType/Main.cshtml", model);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (model.CurrencyId != BaseCurrencyType.All &&
                currencyList.ContainsKey(model.CurrencyId) &&
                model.FromDate.Date == model.ToDate.Date)
            {
                model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                return Excel(model);
            }

            return EmptyExcel();
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            var ticketResult = BetListService.GetBetListData(metaModel, UspKey);

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

            var excel = new BetListExcel(new ShowingOption(), metaModel.LicenseeId, new List<int> { BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "WinLossByBetTypeBetList");
        }
    }
}