// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwoViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.25">Created</change>
// </history>

namespace Fanex.Athena.Models.ViewModel.Forecast
{
    using System.Collections.Generic;
    using DTO.Forecast.Forecast1X2;

    using Fanex.Athena.Models.DTO.Common;

    using MetaModel.Forecast;

    /// <summary>
    /// ForecastOneXTwoViewModel class.
    /// </summary>
    public class ForecastOneXTwoViewModel : ForecastBaseViewModel
    {
        public ForecastOneXTwoMetaModel Param { get; set; }

        public IList<ForecastOneXTwo> MainData { get; set; }

        public IList<League> LeagueList { get; set; }
    }
}