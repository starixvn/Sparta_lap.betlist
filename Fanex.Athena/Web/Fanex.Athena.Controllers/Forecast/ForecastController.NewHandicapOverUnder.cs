// ForecastController.NewHandicapOverUnder.cs--------------------------------
// <copyright file="ForecastController.NewHandicapOverUnder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bravo.Tran" date="2013.08.06">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Logging;
    using Models.MetaModel.Forecast;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast Handicap Over/Under.
    /// </summary>
    public partial class ForecastController
    {
        /// <summary>
        /// Licensee Offer New Forecast List.
        /// </summary>
        private IList<string> _licenseeOfferNewForecastList;

        #region New Forecast

        /// <summary>
        /// New fore cast report.
        /// </summary>
        /// <param name="leagueId">The league id.</param>
        /// <param name="matchId">The match id.</param>
        /// <param name="currencyId">The current id.</param>
        /// <param name="baseCurrency">The base currency.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult NewForecastHdpOu(int leagueId = 0, int matchId = 0, int currencyId = 0, string baseCurrency = null)
        {
            this._licenseeOfferNewForecastList = GetLicenseeOfferNewForecastList();
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            if (this._licenseeOfferNewForecastList.Contains(licenseeId.ToString()))
            {
                var metaModel = new NewForecastHdpOuMetaModel
                {
                    LicenseeId = licenseeId,
                    UserId = SessionService.UserSession.UserId,
                    MatchId = matchId,
                    CurrencyId = currencyId,
                    LeagueId = leagueId,
                };

                if (string.IsNullOrEmpty(baseCurrency))
                {
                    SetConvetedUSDCurrencyName(metaModel);
                }
                else
                {
                    metaModel.BaseCurrency = baseCurrency;
                }

                return View("~/Views/Forecast/NewForecastHdpOu/Main.cshtml", this.GetForecastHdpOuViewModel(metaModel));
            }
            else
            {
                return RedirectToAction("ForecastHandicapOverUnder");
            }
        }

        /// <summary>
        /// New fore cast report.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult NewForecastHdpOu(NewForecastHdpOuMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            return View("~/Views/Forecast/NewForecastHdpOu/Main.cshtml", this.GetForecastHdpOuViewModel(metaModel));
        }

        /// <summary>
        /// Get Match List By League.
        /// </summary>
        /// <param name="leagueId">The league Id.</param>
        /// <returns>The view result.</returns>
        [HttpPost]
        public ViewResult GetMatchListByLeague(int leagueId)
        {
            var matchList = _matchService.GetForecastMatchListByLeague(leagueId).OrderBy(x => x.MatchName).ToList();
            return View("~/Views/Forecast/NewForecastHdpOu/MatchOptionList.cshtml", matchList);
        }

        #endregion New Forecast

        #region Utility

        /// <summary>
        /// Get offered licensee.
        /// </summary>
        /// <returns>The licensee id list.</returns>
        private static IList<string> GetLicenseeOfferNewForecastList()
        {
            var items = AppSettings.GetValue("LicenseeOfferNewForecast");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new List<string>();
        }

        /// <summary>
        /// Get New Forecast Handicap Over/Under ViewModel.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The view model.</returns>
        [SuppressMessage(
                  "StyleCop.CSharp.Nexcel.NexcelCustomRules",
                  "SP2101:MethodMustNotContainMoreLinesThan",
                  Justification = "Reviewed. Suppression is OK here.")]
        private NewForecastHdpOuViewModel GetForecastHdpOuViewModel(NewForecastHdpOuMetaModel metaModel)
        {
            IList<Match> matchList = null;
            IList<League> leagueList = null;
            var headerInfo = new Models.DTO.Forecast.NewForecastHdpOu.NewForecastHdpOuMatchInfo();
            var model = new NewForecastHdpOuViewModel
            {
                Param = metaModel,
                LeagueList = leagueList,
                MatchList = matchList,
                HeaderInfo = headerInfo,
                MainData = new NewForecastHdpOuData(),
                CurrencyList = SessionService.UserSession.CurrencyList,
            };
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    model.LeagueList = _forecastService.GetFcRunningLeague(metaModel);
                    if (model.LeagueList.Count > 0)
                    {
                        metaModel.LeagueId = GetLeagueId(metaModel.LeagueId, model.LeagueList);
                        if (metaModel.LeagueId > 0)
                        {
                            model.MatchList = _matchService.GetForecastMatchListByLeague(metaModel.LeagueId).OrderBy(x => x.MatchName).ToList();

                            if (model.MatchList.Count > 0)
                            {
                                /*get current match or first match*/
                                var match = model.MatchList.Where(l => l.MatchId == metaModel.MatchId).SingleOrDefault();
                                if (match == null)
                                {
                                    match = model.MatchList.FirstOrDefault();
                                }

                                if (match != null)
                                {
                                    metaModel.MatchId = match.MatchId;
                                    metaModel.HomeTeamId = match.HomeId;
                                    metaModel.AwayTeamId = match.AwayId;
                                    metaModel.IsNeutral = match.IsNeutral;
                                }
                                else
                                {
                                    metaModel.MatchId = -1;
                                }
                            }
                        }
                    }

                    model.Param = metaModel;
                }

                metaModel.HomeTeamName = CacheRef.GetTeamName(metaModel.HomeTeamId) + (metaModel.IsNeutral ? " (n)" : string.Empty);
                metaModel.AwayTeamName = CacheRef.GetTeamName(metaModel.AwayTeamId);
                model.MainData = _forecastService.GetNewForecastData(metaModel, headerInfo);
                model.HeaderInfo = headerInfo;
            }
            catch (System.Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return model;
        }

        #endregion Utility
    }
}