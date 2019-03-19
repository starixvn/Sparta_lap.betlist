// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetList.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(
        PermissionKeys.ReportLicenseeWinLoss,
        PermissionKeys.ReportWinLossByProduct,
        PermissionKeys.ReportColossusBetsWinLoss)]
    [BetListAuthorization]
    public class LicenseeWinLossBetListController : BetListBaseController
    {
        private static readonly int[] HiddenColumns = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

        public LicenseeWinLossBetListController(
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
            CreateDefaultMetaModel(metaModel);
            return await ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/LicenseeWl/Main.cshtml", "Lrf_BetList_SelWinloss_Mem").ConfigureAwait(false);
        }

        protected override async Task<ActionResult> ExportExcel(BetListExcelMetaModel metaModel)
        {
            var curencyId = metaModel.IsConvert ? metaModel.BaseCurrencyId : metaModel.CurrencyId;
            metaModel.Title = GetCustomerBetListTitle(metaModel, GetViewInCurrency(curencyId, metaModel.CurrencyName));

            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            metaModel.HiddenColumns = HiddenColumns;
            var fileName = string.IsNullOrWhiteSpace(metaModel.ExcelFileName) ? "LicenseeWinLossBetList" : metaModel.ExcelFileName;

            return await ProcessExportExcel(metaModel, fileName).ConfigureAwait(false);
        }
    }
}