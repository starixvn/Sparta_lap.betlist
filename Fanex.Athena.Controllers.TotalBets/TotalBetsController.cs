// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.Controllers.TotalBets
{
    using Service.TotalBet;
    using Service.TotalBet.Interfaces;

    /// <summary>
    /// Total bets controller.
    /// </summary>
    public partial class TotalBetsController : ReadonlyBaseController
    {
        #region Field

        /// <summary>
        /// Handicap OU service.
        /// </summary>
        private readonly IHandicapOverUnderService _handicapOverUnderService;

        /// <summary>
        /// Money Line service.
        /// </summary>
        private readonly IMoneyLineService _moneyLineService;

        /// <summary>
        /// Odd/Even + 1 X 2 + Draw/No Draw service.
        /// </summary>
        private readonly IOddEvenDrawNoDrawService _oddEvenDrawNoDrawService;

        /// <summary>
        /// Total Goal service.
        /// </summary>
        private readonly ITotalGoalService _totalGoalService;

        /// <summary>
        /// Outright service.
        /// </summary>
        private readonly IOutrightService _outrightService;

        /// <summary>
        /// Half Time Full Time service.
        /// </summary>
        private readonly IHalfTimeFullTimeService _halfTimeFullTimeService;

        /// <summary>
        /// First Goal/Last Goal service.
        /// </summary>
        private readonly IFirstGoalLastGoalService _firstGoalLastGoalService;

        /// <summary>
        /// Home/Draw/Away No Bet service.
        /// </summary>
        private readonly IHomeDrawAwayNoBetService _homeDrawAwayNoBetService;

        /// <summary>
        /// Racing service.
        /// </summary>
        private readonly IRacingService _racingService;

        /// <summary>
        /// Number Game service.
        /// </summary>
        private readonly INumberGameService _numberGameService;

        #endregion Field

        #region Constructor

        public TotalBetsController()
        {
            this._handicapOverUnderService = new HandicapOverUnderService();

            this._moneyLineService = new MoneyLineService();

            this._oddEvenDrawNoDrawService = new OddEvenDrawNoDrawService();

            this._totalGoalService = new TotalGoalService();

            this._outrightService = new OutrightService();

            this._halfTimeFullTimeService = new HalfTimeFullTimeService();

            this._firstGoalLastGoalService = new FirstGoalLastGoalService();

            this._homeDrawAwayNoBetService = new HomeDrawAwayNoBetService();

            this._racingService = new RacingService();

            this._numberGameService = new NumberGameService();
        }

        #endregion Constructor
    }
}