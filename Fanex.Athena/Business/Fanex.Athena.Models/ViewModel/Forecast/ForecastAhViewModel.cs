// --------------------------------------------------------------------------
// <copyright file="ForecastAhViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Models.ViewModel.Forecast
{
    using DTO.Forecast.ForecastAH;
    using MetaModel.Forecast;

    /// <summary>
    /// ForecastAhViewModel class.
    /// </summary>
    public class ForecastAhViewModel
    {
        public ForecastAhMetaModel Param { get; set; }

        public ForecastAh ForecastHeaderParam { get; set; }

        public ForecastValue ForecastValue { get; set; }
    }
}