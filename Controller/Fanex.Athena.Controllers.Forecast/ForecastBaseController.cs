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
    using System.Collections.Generic;
    using System.Linq;
    using Extension.Utility;
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

        protected static IList<string> GetLicenseeOfferNewForecastList()
        {
            var items = AppSettings.GetValue("LicenseeOfferNewForecast");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new List<string>();
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
    }
}