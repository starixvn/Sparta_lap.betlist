// --------------------------------------------------------------------------
// <copyright file="ForecastAh.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Model.Forecast.DTO.ForecastAH
{
    using System.Collections.Generic;
    using Model.Core.Helper;

    /// <summary>
    /// ForecastAh class.
    /// </summary>
    public class ForecastAH
    {
        private string _homeTeamName;

        private string _awayTeamName;

        public string ForecastName { get; set; }

        public List<string> ForecastLink { get; set; }

        public bool IsNeutral { get; set; }

        public int HomeId { get; set; }

        public string HomeTeamName
        {
            get
            {
                return _homeTeamName ??
                       (_homeTeamName = CacheRef.GetTeamName(HomeId) + (IsNeutral ? " (N)" : string.Empty));
            }
        }

        public int AwayId { get; set; }

        public string AwayTeamName
        {
            get
            {
                return _awayTeamName ?? (_awayTeamName = CacheRef.GetTeamName(AwayId));
            }
        }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }
    }
}