// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwoViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.25">Created</change>
// </history>

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;
    using DTO.Forecast1X2;
    using Fanex.Athena.Model.Core.DTO.Common;
    using MetaModel;

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