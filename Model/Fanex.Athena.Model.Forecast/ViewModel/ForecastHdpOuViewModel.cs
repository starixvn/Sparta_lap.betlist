// -----------------------------------------------------------------------
// <copyright file="ForecastHdpOuViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;
    using DTO.ForecastHdou;
    using Fanex.Athena.Model.Core.DTO.Common;
    using MetaModel;

    public class ForecastHdpOuViewModel : ForecastBaseViewModel
    {
        public ForecastHandicapOverUnderMetaModel Param { get; set; }

        public IList<ForecastHandicapOverUnder> MainData { get; set; }

        public IEnumerable<League> LeagueList { get; set; }
    }
}