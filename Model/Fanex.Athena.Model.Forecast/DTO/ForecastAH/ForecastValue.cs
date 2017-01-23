// --------------------------------------------------------------------------
// <copyright file="ForecastValue.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO.ForecastAH
{
    using System.Collections.Generic;

    /// <summary>
    /// ForecastValue class.
    /// </summary>
    public class ForecastValue
    {
        public IList<string> ForeCastLink { get; set; }

        public IList<int> ForecastHeader { get; set; }

        public decimal ForeCastValue0 { get; set; }

        public decimal ForeCastValue1 { get; set; }

        public decimal ForeCastValue2 { get; set; }

        public decimal ForeCastValue3 { get; set; }

        public decimal ForeCastValue4 { get; set; }

        public bool HasData { get; set; }
    }
}