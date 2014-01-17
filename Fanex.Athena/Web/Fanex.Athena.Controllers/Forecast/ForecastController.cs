// --------------------------------------------------------------------------
// <copyright file="ForecastController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bravo.Tran" date="2013.08.13">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Common;
    using Service.Implement.Forecast;
    using Service.Interface.Forecast;

    /// <summary>
    /// Forecast controller.
    /// </summary>
    public partial class ForecastController : ReadonlyBaseController
    {
        #region Fields

        /// <summary>
        /// Forecast service.
        /// </summary>
        private readonly IForecastService _forecastService;

        private readonly IMatchService _matchService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ForecastController" /> class.
        /// </summary>
        public ForecastController()
        {
            this._forecastService = new ForecastService();
            this._matchService = new MatchService();
        }

        #endregion Constructor
    }
}