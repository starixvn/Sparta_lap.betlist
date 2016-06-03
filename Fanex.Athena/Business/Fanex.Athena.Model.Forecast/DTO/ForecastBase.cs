// --------------------------------------------------------------------------
// <copyright file="ForecastBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Model.Forecast.DTO
{
    using Model.Core.Helper;

    /// <summary>
    /// ForecastBase class.
    /// </summary>
    public class ForecastBase
    {
        private string _homeTeamName;
        private string _awayTeamName;

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

        public int MatchId { get; set; }

        public int MatchCode { get; set; }

        public string ShowTime { get; set; }

        public bool IsNeutral { get; set; }

        public bool LiveIndicator { get; set; }

        public byte LiveAwayScore { get; set; }

        public byte LiveHomeScore { get; set; }
    }
}