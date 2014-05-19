// --------------------------------------------------------------------------
// <copyright file="ForecastController.HandicapOverUnder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.Forecast;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast Handicap Over/Under.
    /// </summary>
    public partial class ForecastController
    {
        /// <summary>
        /// Get Forecast Handicap Over/Under View.
        /// </summary>
        /// <returns>Action Result.</returns>
        [FunctionAuthorization]
        public ActionResult ForecastHandicapOverUnder()
        {
            var liceseeSession = SessionService.LicenseeSession;
            this._licenseeOfferNewForecastList = this.GetLicenseeOfferNewForecastList();

            if (_licenseeOfferNewForecastList.Contains(liceseeSession.LicenseeId.ToString()))
            {
                return this.RedirectToAction("NewForecastHdpOu");
            }

            var metaModel = new ForecastHdpOuMetaModel
            {
                LicenseeId = liceseeSession.LicenseeId,
                UserId = SessionService.UserSession.UserId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = this.GetForecastViewModel(metaModel);

            return View("~/Views/Forecast/ForecastHDPOU/Main.cshtml", model);
        }

        /// <summary>
        /// Get Forecast Handicap Over/Under View.
        /// </summary>
        /// <param name="metaModel">Meta Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult ForecastHandicapOverUnder(ForecastHdpOuMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            var model = this.GetForecastViewModel(metaModel);

            return View("~/Views/Forecast/ForecastHDPOU/Main.cshtml", model);
        }

        #region Utility

        /// <summary>
        /// Get Forecast View Model.
        /// </summary>
        /// <param name="metaModel">Meta Model.</param>
        /// <returns>ForecastHandicapOverUnder ViewModel.</returns>
        private ForecastHdpOuViewModel GetForecastViewModel(ForecastHdpOuMetaModel metaModel)
        {
            var leagueList = _forecastService.GetFcRunningLeague(metaModel);

            var currentLeagueId = metaModel.LeagueId;
            metaModel.LeagueId = GetLeagueId(currentLeagueId, leagueList);

            var forecastHdpOuByLeague = _forecastService.GetFcHandicapOverUnder(metaModel);

            return new ForecastHdpOuViewModel
                {
                    Param = metaModel,
                    MainData = forecastHdpOuByLeague,
                    LeagueList = leagueList,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    CurrencyList = SessionService.UserSession.CurrencyList
                };
        }

        #endregion Utility
    }
}