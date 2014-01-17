// --------------------------------------------------------------------------
// <copyright file="ForecastController.OneXTwo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.25">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Models.MetaModel.Forecast;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast One X Two controller.
    /// </summary>
    public partial class ForecastController
    {
        /// <summary>
        /// Get Forecast OneXTwo.
        /// </summary>
        /// <returns>Action Result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult ForecastOneXTwo()
        {
            var metaModel = new ForecastOneXTwoMetaModel
                {
                    LicenseeId = SessionService.LicenseeSession.LicenseeId,
                };
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";

            var model = this.GetForecastViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        /// <summary>
        /// Post Forecast OneXTwo.
        /// </summary>
        /// <param name="metaModel">Meta Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult ForecastOneXTwo(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            var model = this.GetForecastViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        #region Forecast Pop up

        /// <summary>
        /// Get Forecast OneXTwo Popup.
        /// </summary>
        /// <param name="metaModel">Meta Model.</param>
        /// <returns>Action Result.</returns>
        public ActionResult ForecastOneXTwoPopup(ForecastOneXTwoMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            var model = this.GetForecastViewModel(metaModel);

            return View("~/Views/Forecast/Forecast1X2/Main.cshtml", model);
        }

        #endregion Forecast Pop up

        #region Utility

        /// <summary>
        /// Get League Id.
        /// </summary>
        /// <param name="leagueId">The league Id.</param>
        /// <param name="leagueList">League List.</param>
        /// <returns>The league id.</returns>
        public int GetLeagueId(int leagueId, IList<League> leagueList)
        {
            if (
                leagueList.Any(l => l.LeagueId == leagueId))
            {
                return leagueId;
            }
            else
            {
                var first = leagueList.FirstOrDefault();
                return first != null ? first.LeagueId : -1;
            }
        }

        /// <summary>
        /// Get Forecast ViewModel.
        /// </summary>
        /// <param name="metaModel">Meta Model.</param>
        /// <returns>ForecastOneXTwo ViewModel.</returns>
        private ForecastOneXTwoViewModel GetForecastViewModel(ForecastOneXTwoMetaModel metaModel)
        {
            var model = new ForecastOneXTwoViewModel();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var leagueList = _forecastService.GetFcRunningLeague(metaModel);
            var currentLeagueId = metaModel.LeagueId;

            if (metaModel.MatchId != 0)
            {
                var forecastOneXTwoByMatch = _forecastService.GetFcOneXTwoByMatch(metaModel, SessionService.UserSession.UserId);
                model.MainData = forecastOneXTwoByMatch;
            }
            else
            {
                metaModel.LeagueId = this.GetLeagueId(currentLeagueId, leagueList);

                var forecastcOneXTwoByLeague = _forecastService.GetFcOneXTwoByLeague(metaModel);
                model.MainData = forecastcOneXTwoByLeague;
            }

            model.Param = metaModel;
            model.LeagueList = leagueList;
            model.LicenseeTypeList = SessionService.LicenseeSession.LicenseeType;
            model.CurrencyList = SessionService.UserSession.CurrencyList;

            return model;
        }

        #endregion Utility
    }
}