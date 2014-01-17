// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.24">Created</change>
// </history>

namespace Fanex.Athena.Models.DTO.Forecast.Forecast1X2
{
    /// <summary>
    /// Forecast 1x2 class.
    /// </summary>
    public class ForecastOneXTwo : ForecastBase
    {
        public decimal HomeWinLoss { get; set; }

        public decimal DrawWinLoss { get; set; }

        public decimal AwayWinLoss { get; set; }

        public decimal HalfTimeHomeWinLoss { get; set; }

        public decimal HalfTImeDrawWinLoss { get; set; }

        public decimal HalfTimeAwayWinLoss { get; set; }
    }
}