// --------------------------------------------------------------------------
// <copyright file="ForecastController.Ah.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bravo.Tran" date="2013.08.13">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Forecast.ForecastAH;
    using Models.MetaModel.Forecast;
    using Models.ViewModel;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast AH.
    /// Display in popup.
    /// </summary>
    public partial class ForecastController
    {
        /// <summary>
        /// Get ForeCastAh View.
        /// </summary>
        /// <param name="metaModel">The meta Model.</param>
        /// <returns>Action Result.</returns>
        public ActionResult ForeCastAH(ForecastAHMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var model = this.GetForecastViewModel(metaModel);

            return View("~/Views/Forecast/ForecastAH/Main.cshtml", model);
        }

        #region Utility

        /// <summary>
        /// Render Forecast Link.
        /// </summary>
        /// <param name="betType">Bet type value.</param>
        /// <param name="currentScore">The current Score.</param>
        /// <returns>The key core link.</returns>
        private static List<string> RenderForecastLink(int betType, int currentScore)
        {
            var forecastLink = new List<string>();
            var startIndex = currentScore + 5;
            var endIndex = currentScore - 5;

            if (betType == (int)BetType.OverUnder || betType == (int)BetType.FirstHalfOverUnder)
            {
                endIndex = Math.Max(0, endIndex);
            }

            for (var i = startIndex; i >= endIndex; i--)
            {
                forecastLink.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            return forecastLink;
        }

        /// <summary>
        /// Render Forecast Header.
        /// </summary>
        /// <param name="betType">The bet type.</param>
        /// <param name="currentScore">The current score.</param>
        /// <returns>The key core list.</returns>
        private static List<int> RenderForecastHeader(int betType, int currentScore)
        {
            var forecastHeader = new List<int>();
            var startIndex = betType == (int)BetType.Handicap || betType == (int)BetType.FirstHalfHandicap ? currentScore + 2 : currentScore + 4;
            var endIndex = startIndex - 4;

            for (int i = endIndex; i <= startIndex; i++)
            {
                forecastHeader.Add(i);
            }

            return forecastHeader;
        }

        /// <summary>
        /// Get Forecast View Model.
        /// </summary>
        /// <param name="metaModel">Meta Model.</param>
        /// <returns>ForecastAh View Model.</returns>
        private ForecastAHViewModel GetForecastViewModel(ForecastAHMetaModel metaModel)
        {
            var forecastHeaderParam = _forecastService.GetFcTopOddsByMatch(metaModel.MatchId, metaModel.BetType);
            ForecastValue forecastValue = null;
            var userSession = SessionService.UserSession;
            if (forecastHeaderParam != null)
            {
                /* Set default currentScore*/
                if (Request.QueryString["CurrentScore"] == null)
                {
                    if (metaModel.BetType == (int)BetType.OverUnder || metaModel.BetType == (int)BetType.FirstHalfOverUnder)
                    {
                        // Overunder
                        metaModel.CurrentScore = forecastHeaderParam.LiveHomeScore + forecastHeaderParam.LiveAwayScore;
                    }
                    else
                    {
                        metaModel.CurrentScore = forecastHeaderParam.LiveHomeScore - forecastHeaderParam.LiveAwayScore;
                    }
                }

                /*Handicap or 1st Handicap*/
                if (metaModel.BetType == (int)BetType.Handicap || metaModel.BetType == (int)BetType.FirstHalfHandicap)
                {
                    forecastHeaderParam.ForecastName = metaModel.BetType == (int)BetType.Handicap ? ViewResources.Handicap : ViewResources.lbl1stHandicap;
                    forecastValue = _forecastService.GetForecastHandicap(metaModel, userSession.UserId);
                }
                else
                {
                    /* Over/Under or 1st Over/Under*/
                    forecastHeaderParam.ForecastName = metaModel.BetType == (int)BetType.OverUnder ? ViewResources.OverUnder : ViewResources.lbl1stOver_Under;
                    forecastValue = _forecastService.GetForacastOverUnder(metaModel, userSession.UserId);
                }

                forecastValue.ForeCastLink = RenderForecastLink(metaModel.BetType, metaModel.CurrentScore);
                forecastValue.ForecastHeader = RenderForecastHeader(metaModel.BetType, metaModel.CurrentScore);
            }

            return new ForecastAHViewModel
            {
                Param = metaModel,
                ForecastHeaderParam = forecastHeaderParam,
                ForecastValue = forecastValue
            };
        }

        #endregion Utility
    }
}