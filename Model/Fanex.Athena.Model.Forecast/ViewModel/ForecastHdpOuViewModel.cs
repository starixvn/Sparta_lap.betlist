// -----------------------------------------------------------------------
// <copyright file="ForecastHdpOuViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;
    using DTO.ForecastHdou;
    using Fanex.Athena.Model.Core.DTO.Common;
    using MetaModel;

    /// <summary>
    /// Forecast HDP OU ViewModel class.
    /// </summary>
    public class ForecastHdpOuViewModel : ForecastBaseViewModel
    {
        public ForecastHandicapOverUnderMetaModel Param { get; set; }

        public IList<ForecastHandicapOverUnder> MainData { get; set; }

        public IList<League> LeagueList { get; set; }
    }
}