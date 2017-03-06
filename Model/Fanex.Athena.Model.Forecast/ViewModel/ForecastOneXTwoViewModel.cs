// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwoViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;
    using DTO.Forecast1X2;
    using Fanex.Athena.Model.Core.DTO.Common;
    using MetaModel;

    public class ForecastOneXTwoViewModel : ForecastBaseViewModel
    {
        public ForecastOneXTwoMetaModel Param { get; set; }

        public IList<ForecastOneXTwo> MainData { get; set; }

        public IEnumerable<League> LeagueList { get; set; }
    }
}