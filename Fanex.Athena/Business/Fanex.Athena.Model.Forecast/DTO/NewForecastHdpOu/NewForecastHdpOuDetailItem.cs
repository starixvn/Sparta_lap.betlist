// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuDetailItem.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO.NewForecastHdpOu
{
    /// <summary>
    /// NewForecast HDP OU DetailItem class.
    /// </summary>
    public class NewForecastHdpOuDetailItem
    {
        public int Bettype { get; set; }

        public decimal Hdp { get; set; }

        public decimal HomeWinLoss { get; set; }

        public decimal AwayWinLoss { get; set; }
    }
}