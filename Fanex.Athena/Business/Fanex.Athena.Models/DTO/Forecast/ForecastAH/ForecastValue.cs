// --------------------------------------------------------------------------
// <copyright file="ForecastValue.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Models.DTO.Forecast.ForecastAH
{
    using System.Collections.Generic;

    /// <summary>
    /// ForecastValue class.
    /// </summary>
    public class ForecastValue
    {
        public List<string> ForeCastLink { get; set; }

        public List<int> ForecastHeader { get; set; }

        public decimal ForeCastValue0 { get; set; }

        public decimal ForeCastValue1 { get; set; }

        public decimal ForeCastValue2 { get; set; }

        public decimal ForeCastValue3 { get; set; }

        public decimal ForeCastValue4 { get; set; }

        public bool HasData { get; set; }
    }
}