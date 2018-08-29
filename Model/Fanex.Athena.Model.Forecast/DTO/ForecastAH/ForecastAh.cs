// --------------------------------------------------------------------------
// <copyright file="ForecastAh.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO.ForecastAH
{
    using System.Collections.Generic;

    public class ForecastAH : ForecastBase
    {
        public string ForecastName { get; set; }

        public IList<string> ForecastLink { get; set; }
    }
}