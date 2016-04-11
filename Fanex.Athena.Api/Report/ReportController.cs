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
    public partial class ReportController : ApiReportBaseController
    {        
        private readonly IPlatformWinLossService _platformWinLossService;

        private readonly IWinLossByProductService _winlossByProductService;

        private readonly IWinLossByLeagueService _winlossByLeagueService;

        private readonly IBalanceUpService _balanceUpService;

        private readonly IAgCasinoWinLossService _agcasinoWinLossService;

        private readonly IActiveCustomerService _activeCustomerService;

        private readonly IGoldDeluxeWinLossService _golddeluxeWinLossService;

        private readonly IThirdPartyWinLossService _thirdPartyWinLossService;

        private readonly IAgCasinoBalanceService _agcasinoBalanceService;

        private readonly IMicroGamingWinLossService _microGamingWinLossService;

        public ReportController()
        {
            _platformWinLossService = new PlatformWinLossService();
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
    }
}