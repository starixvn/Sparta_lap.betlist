// -----------------------------------------------------------------------
// <copyright file="ForecastHdpOuViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Forecast
{
    using System.Collections.Generic;
    using DTO.Forecast.ForecastHdou;

    using Fanex.Athena.Models.DTO.Common;

    using MetaModel.Forecast;

    /// <summary>
    /// Forecast HDP OU ViewModel class.
    /// </summary>
    public class ForecastHdpOuViewModel : ForecastBaseViewModel
    {
        public ForecastHdpOuMetaModel Param { get; set; }

        public IList<ForecastHandicapOverUnder> MainData { get; set; }

        public IList<League> LeagueList { get; set; }
    }
}