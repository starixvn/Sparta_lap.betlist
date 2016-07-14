// --------------------------------------------------------------------------
// <copyright file="ForecastAhViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

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