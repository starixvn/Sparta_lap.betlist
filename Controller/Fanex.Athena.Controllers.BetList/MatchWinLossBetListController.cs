// -----------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
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
    using Fanex.Athena.Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { BetEffectiveColumn };

        public MatchWinLossBetListController(
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
        public ActionResult MatchBetType(BetListExcelMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, "~/Views/Desktop/BetList/MatchWl/MatchBetType.cshtml");
        }

        [HttpGet]
        public ActionResult MixParlay(BetListExcelMetaModel metaModel)
        {
            return ProcessRequest(metaModel, "Lrf_BetList_MatchWL_Parlay", "~/Views/Desktop/BetList/MatchWl/MixParlay.cshtml");
        }

        [HttpGet]
        public ActionResult Outright(BetListExcelMetaModel metaModel)
        {
            return ProcessRequest(metaModel, "Lrf_BetList_MatchWL_Outright", "~/Views/Desktop/BetList/MatchWl/Outright.cshtml");
        }

        private ActionResult ProcessRequest(BetListExcelMetaModel metaModel, string uspKey, string viewPath)
        {
            CreateDefaultMetaModel(metaModel);
            metaModel.IsServerPaging = true;
            return ProcessDefaultRequest(metaModel, viewPath, uspKey);
        }

        protected override bool IsValidExportExcel(BetListMetaModel model)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            return model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId);
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;

            if (string.IsNullOrEmpty(metaModel.UspKey))
            {
                metaModel.UspKey = "Lrf_BetList_MatchWL_Bettype";
            }
            metaModel.ShowingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                  ? CacheRefAdapter.GetTeamName(metaModel.HomeId) + " vs " + CacheRefAdapter.GetTeamName(metaModel.AwayId) + " - "
                  : string.Empty;
            var currencyTitle = GetViewInCurrency(metaModel);

            metaModel.Title = ViewResources.BetList
                + string.Format(
                " - {0}{1} - {2}",
                matchName + (metaModel.BetType > 0 ? CacheRefAdapter.GetBettype(metaModel.BetType) + " - " : string.Empty),
                metaModel.FromDate.ToShortDateString(),
                currencyTitle);
            metaModel.HiddenColumns = HiddenColumns;

            return ProcessExportExcel(metaModel, "MatchWinLossBetList");
        }
    }
}