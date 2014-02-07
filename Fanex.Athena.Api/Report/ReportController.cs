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
        }

        private void RedirectToAccessDenied()
        {
            HttpContext.Current.Response.Redirect("~/AccessDenied.aspx");
        }
    }
}