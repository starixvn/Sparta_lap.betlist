// -----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController : ReadonlyBaseController
    {
        #region Constant

        public const int PageSizeExcel = int.MaxValue - 1000;

        #endregion Constant

        #region Fields

        private readonly IBalanceUpService _balanceUpService;
        private readonly ICreditBalanceService _creditBalanceService;
        private readonly ICustBalanceService _custBalanceService;
        private readonly ICustPendingFundsService _custPendingFundsService;
        private readonly ICustTurnoverService _custTurnoverService;
        private readonly ICustWinlossService _custWinlossService;
        private readonly IDailyRacingWinLossService _dailyRacingWinLossService;
        private readonly ILiveReportService _liveReportService;
        private readonly IMatchWinLossService _matchWinLossService;
        private readonly IProductService _productService;
        private readonly IRacingCommissionService _racingCommissionService;
        private readonly ISettledMatchCountService _settledMatchCountService;
        private readonly ITurnoverService _turnoverService;
        private readonly IWinLossByBetTypeServive _winLossByBetTypeService;
        private readonly IBetTypeService _betTypeService;

        private readonly ISportTypeService _sportTypeService;
        private readonly ILeagueService _leagueService;
        private readonly IMatchService _matchService;

        #endregion Fields

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        public ReportController()
            : base()
        {
            this._balanceUpService = new BalanceUpService();

            this._matchWinLossService = new MatchWinLossService();
            this._productService = new ProductService();
            this._dailyRacingWinLossService = new DailyRacingWinLossService();
            this._liveReportService = new LiveReportService();
            this._turnoverService = new TurnoverService();
            this._winLossByBetTypeService = new WinLossByBetTypeService();
            this._custTurnoverService = new CustomerTurnoverService();
            this._custBalanceService = new CustomerBalanceService();
            this._custWinlossService = new CustomerWinlossService();
            this._settledMatchCountService = new SettledMatchCountService();
            this._racingCommissionService = new RacingCommissionService();
            this._custPendingFundsService = new CustomerPendingFundsService();
            this._creditBalanceService = new CreditBalanceService();
            _sportTypeService = new SportTypeService(this._balanceUpService.BalanceUpInfo);
            this._betTypeService = new BetTypeService();
            this._leagueService = new LeagueService();
            this._matchService = new MatchService();
        }

        [HttpPost]
        [AjaxRequest]
        public ActionResult GetBettypes(IList<SportProductItem> sports)
        {
            if (sports == null || sports.Count == 0)
            {
                return Json(new List<SportProductItem>());
            }

            return Json(_betTypeService.GetBettypes(SessionService.LicenseeSession.LicenseeId, sports, SessionService.UserSession.Language));
        }

        [HttpGet]
        [AjaxRequest]
        public ActionResult GetSportsAndProducts()
        {
            return Json(
                _sportTypeService.GetSportsAndProducts(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                JsonRequestBehavior.AllowGet);
        }

        [AjaxRequest]
        public ActionResult GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            return Json(
                _leagueService.GetLeaguesBySport(metaModel)
                .Select(x => new { Key = x.LeagueId, Value = x.LeagueName }));
        }

        [AjaxRequest]
        public ActionResult GetMatchesByLeague(int leagueid, DateTime fromDate, DateTime toDate)
        {
            var matches = _matchService.GetMatchesByLeague(leagueid, fromDate, toDate);
            matches.Insert(0, new Match() { MatchName = ViewResources.All, MatchId = 0 });

            return Json(matches.Select(x => new { Key = x.MatchId, Value = x.MatchName }));
        }
    }
}