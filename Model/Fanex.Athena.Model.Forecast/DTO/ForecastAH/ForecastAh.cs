// --------------------------------------------------------------------------
// <copyright file="ForecastAh.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO.ForecastAH
{
    using System.Collections.Generic;

    /// <summary>
    /// ForecastAh class.
    /// </summary>
    public class ForecastAH
    {
        public string ForecastName { get; set; }

        public List<string> ForecastLink { get; set; }

        public bool IsNeutral { get; set; }

        public int HomeId { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayId { get; set; }

        public string AwayTeamName { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }
    }
}