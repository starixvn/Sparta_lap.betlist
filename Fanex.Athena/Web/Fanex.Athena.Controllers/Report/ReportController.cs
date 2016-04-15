// -----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    public partial class ReportController : ReportBaseController
    {
        private readonly ISportTypeService _sportTypeService;
        private readonly ILeagueService _leagueService;
        private readonly IMatchService _matchService;

        public ReportController() : base()
        {
            _sportTypeService = new SportTypeService();
            _leagueService = new LeagueService();
            _matchService = new MatchService();
        }
    }
}