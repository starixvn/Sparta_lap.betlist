// --------------------------------------------------------------------------
// <copyright file="ForecastBase.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO
{
    /// <summary>
    /// ForecastBase class.
    /// </summary>
    public class ForecastBase
    {
        public int HomeId { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayId { get; set; }

        public string AwayTeamName { get; set; }

        public int MatchId { get; set; }

        public int MatchCode { get; set; }

        public string ShowTime { get; set; }

        public bool IsNeutral { get; set; }

        public bool LiveIndicator { get; set; }

        public int LiveAwayScore { get; set; }

        public int LiveHomeScore { get; set; }
    }
}