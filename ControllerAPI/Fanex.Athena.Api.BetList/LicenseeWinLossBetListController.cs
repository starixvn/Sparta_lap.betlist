namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Helper;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using VendorAdapter.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss, PermissionKeys.ReportWinLossByProduct, PermissionKeys.ReportColossusBetsWinLoss)]
    [ApiBetListAuthorization]
    public class LicenseeWinLossBetListController : ApiBetListBaseController
    {
        private readonly IReportSettingService _reportSettingService;

#pragma warning disable S107 // Methods should not have too many parameters

        public LicenseeWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder,
            IReportSettingService reportSettingService)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter, betlistHtmlBuilder)
        {
            _reportSettingService = reportSettingService;
        }

        [HttpPost]
        public async Task<dynamic> Index(BetListMetaModel metaModel)
        {
            if (!IsValidDateRange(metaModel.FromDate, metaModel.ToDate))
            {
                return null;
            }

            var userSession = SessionService.UserSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var currencyList = userSession.CurrencyList;

            if (!KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, licenseeId, metaModel.CurrencyId))
            {
                return null;
            }

            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            return await GetBetListResult(metaModel, UspKey).ConfigureAwait(false);
        }

        private bool IsValidDateRange(DateTime fromDate, DateTime toDate)
        {
            var setting = _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss);
            if (setting == null)
            {
                return true;
            }

            return DateTimeHelper.IsBetween(fromDate, setting.MinDate, setting.MaxDate) &&
                DateTimeHelper.IsBetween(toDate, setting.MinDate, setting.MaxDate);
        }
    }
}