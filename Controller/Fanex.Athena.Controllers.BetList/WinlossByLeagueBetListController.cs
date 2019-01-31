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
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Resources;
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
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(WinLossByLeagueBetListMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/WinlossByLeague/Main.cshtml", "Lrf_BetList_WinlostByLeague_Mem");
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            metaModel.Title = string.Format(
                "{0} - {1} → {2} - {3}{4} - {5}",
                ViewResources.CustomerBetList,
                metaModel.FromDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ToDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                viewinCurrency);
            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            metaModel.HiddenColumns = HiddenColumns;
            return ProcessExportExcel(metaModel, "WinLossByLeagueBetList", "/BetList/ExportExcelWinlossByLeague");
        }
    }
}