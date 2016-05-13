// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Forecast
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu;
    using Fanex.Athena.Models.MetaModel.Forecast;

    /// <summary>
    /// NewForecast HDP OU ViewModel class.
    /// </summary>
    public class NewForecastHdpOuViewModel : ForecastBaseViewModel
    {
        public NewForecastHandicapOverUnderMetaModel Param { get; set; }

        public IList<League> LeagueList { get; set; }

        public IList<Match> MatchList { get; set; }

        public NewForecastHdpOuMatchInfo HeaderInfo { get; set; }

        public NewForecastHdpOuData MainData { get; set; }
    }
}