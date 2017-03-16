// --------------------------------------------------------------------------
// <copyright file="ForecastBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Forecast
{
    using System.Collections.Generic;
    using System.Linq;
    using Model.Core.DTO.Common;
    using Service.Forecast;
    using Service.Forecast.Interfaces;

    public class ForecastBaseController : ReadonlyBaseController
    {
        private readonly IForecastService _forecastService;

        public ForecastBaseController()
        {
            this._forecastService = new ForecastService();
        }

        protected IForecastService ForecastService
        {
            get { return _forecastService; }
        }

        protected static int GetLeagueId(int leagueId, IList<League> leagueList)
        {
            if (leagueList.Any(l => l.LeagueId == leagueId))
            {
                return leagueId;
            }
            else
            {
                var first = leagueList.FirstOrDefault();
                return first != null ? first.LeagueId : -1;
            }
        }

        protected IList<string> GetLicenseeOfferNewForecastList()
        {
            var items = AppSettings.GetValue("LicenseeOfferNewForecast");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new List<string>();
        }
    }
}