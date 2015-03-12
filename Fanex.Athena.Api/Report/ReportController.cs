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
    using System.Web;
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
        }

        private static void RedirectToErrorPage()
        {
            HttpContext.Current.Response.Redirect("~/Utility/Error");
        }

        /// <summary>
        /// Check if the request is valid or not.
        /// Check if the session is valid.
        /// Check if currency and drill down currency is valid.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <returns>Request is valid or not.</returns>
        private bool IsValidRequest(BaseMetaModel metaModel, int currencyId = 0)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            var validModel = (metaModel.CurrencyId <= 0 && metaModel.CurrencyId >= SystemConstants.MinBaseCurrencyType)
                             || currencyList.Values.Contains(metaModel.CurrencyId);
            var validCurrency = (currencyId <= 0 && currencyId >= SystemConstants.MinBaseCurrencyType)
                                 || currencyList.Values.Contains(currencyId);

            return validModel && validCurrency;
        }
    }
}