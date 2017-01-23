// --------------------------------------------------------------------------
// <copyright file="ForecastAhViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using DTO.ForecastAH;
    using MetaModel;

    /// <summary>
    /// ForecastAhViewModel class.
    /// </summary>
    public class ForecastAHViewModel
    {
        public ForecastAHMetaModel Param { get; set; }

        public ForecastAH ForecastHeaderParam { get; set; }

        public ForecastValue ForecastValue { get; set; }
    }
}