// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportPlatformWinLoss)]
    public class PlatformWinLossBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

        public PlatformWinLossBetListController(
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
        public ActionResult Index(BetListExcelMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/PlatformWinLoss/Main.cshtml", "Lrf_BetList_PlatformWinLoss_Mem");
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            var viewinCurrency = GetViewInCurrency(curencyId, metaModel.CurrencyName);
            var platformText = PlatformType.GetPlatformName(metaModel.PlatformType);

            metaModel.Title = ViewResources.Customer + " " + ViewResources.BetList
                + string.Format(
                " - {0} --> {1} - {2}{3} - {4} - {5}",
                metaModel.FromDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ToDate.ToString(FormatKey.ShortDateFormat),
                metaModel.ExternalId,
                metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                ViewResources.Platform + ": " + platformText,
                viewinCurrency);

            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            metaModel.HiddenColumns = HiddenColumns;
            return ProcessExportExcel(metaModel, "PlatformWinLossBetList");
        }
    }
}