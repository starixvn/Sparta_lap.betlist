// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuMatchInfo.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu
{
    /// <summary>
    /// NewForecast HDP OU MatchInfo class.
    /// </summary>
    public class NewForecastHdpOuMatchInfo
    {
        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string KickOffDateString { get; set; }
    }
}