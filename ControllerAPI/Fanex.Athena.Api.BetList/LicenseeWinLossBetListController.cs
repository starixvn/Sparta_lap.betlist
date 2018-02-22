namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Web.Http;
    using Base.Attributes;
    using Extension.Helper;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Entities;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using VendorAdapter.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss, PermissionKeys.ReportWinLossByProduct, PermissionKeys.ReportColossusBetsWinLoss)]
    [ApiBetListAuthorization]
    public class LicenseeWinLossBetListController : ApiBetListBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        public LicenseeWinLossBetListController()
        {
            _reportSettingService = new ReportSettingService();
        }

        [CLSCompliant(false)]
        public LicenseeWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter)
        : base(sessionService, productService, languageService, betListService, balanceUpDataAccess, ticketHelper, cacheRefAdapter)
        {
            _reportSettingService = new ReportSettingService();
        }

        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
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

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }

        private bool IsValidDateRange(DateTime fromDate, DateTime toDate)
        {
            var setting = _reportSettingService.GetSetting(SessionService.LicenseeSession.LicenseeName, PermissionKeys.ReportLicenseeWinLoss);
            if (setting == null)
            {
                return true;
            }

            return DateTimeHelper.IsBetween(fromDate, setting.MinDate, setting.MaxDate) &&
                DateTimeHelper.IsBetween(toDate, setting.MinDate, setting.MaxDate);
        }
    }
}