// --------------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.Report
{
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// LicenseeWLController class.
    /// </summary>
    public partial class ReportController : BaseApiController
    {
        /// <summary>
        /// Licensee Win Loss Service.
        /// </summary>
        private readonly ILicenseeWinLossService _licenseeWinLossService;

        /// <summary>
        /// Mobile win loss service.
        /// </summary>
        private readonly IMobileWinLossService _mobileWinLossService;

        /// <summary>
        /// Licensee Win Loss Service.
        /// </summary>
        private readonly IWinLossByProductService _winlossByProductService;

        private readonly IWinLossByLeagueService _winlossByLeagueService;

        private readonly IBalanceUpService _balanceUpService;

        private readonly IAgCasinoWinLossService _agcasinoWinLossService;

        private readonly IActiveCustomerService _activeCustomerService;

        private readonly IGoldDeluxeWinLossService _golddeluxeWinLossService;

        private readonly IThirdPartyWinLossService _thirdPartyWinLossService;

        private readonly IAgCasinoBalanceService _agcasinoBalanceService;

        private readonly IMicroGamingWinLossService _microGamingWinLossService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        public ReportController()
        {
            _licenseeWinLossService = new LicenseeWinLossService();
            _mobileWinLossService = new MobileWinLossService();
            _winlossByProductService = new WinLossByProductService();
            _winlossByLeagueService = new WinLossByLeagueService();
            _balanceUpService = new BalanceUpService();
            _agcasinoWinLossService = new AgCasinoWinLossService();
            _activeCustomerService = new ActiveCustomerService();
            _golddeluxeWinLossService = new GoldDeluxeWinLossService();
            _thirdPartyWinLossService = new ThirdPartyWinLossService();
            _agcasinoBalanceService = new AgCasinoBalanceService();
            _microGamingWinLossService = new MicroGamingWinLossService();
        }

        /// <summary>
        /// Check if the request is valid or not.
        /// Check if the session is valid.
        /// Check if currency and drill down currency is valid.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <returns>Request is valid or not.</returns>
        protected bool IsValidRequest(BaseMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            var validModel = (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= SystemConstants.MinBaseCurrencyType)
                             || currencyList.Values.Contains(metaModel.CurrencyId)
                             || (SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean && currencyId == CurrencyType.Cny);
            var validCurrency = IsValidCurrencyRequest(currencyId, currencyList);
            return validModel && validCurrency;
        }

        private bool IsValidCurrencyRequest(int currencyId, System.Collections.Generic.IDictionary<string, int> currencyList)
        {
            var validCurrency = (currencyId <= 0 && currencyId >= SystemConstants.MinBaseCurrencyType) ||
                                currencyList.Values.Contains(currencyId);

            return validCurrency || (SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean && currencyId == CurrencyType.Cny);
        }
    }
}