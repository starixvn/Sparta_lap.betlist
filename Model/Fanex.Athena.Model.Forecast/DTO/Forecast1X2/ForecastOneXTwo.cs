// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO.Forecast1X2
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

        public bool IsExtra { get; set; }
    }
}