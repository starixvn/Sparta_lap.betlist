// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuTotalItem.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu
{
    /// <summary>
    /// NewForecast HDP OU TotalItem class.
    /// </summary>
    public class NewForecastHdpOuTotalItem
    {
        public int Type { get; set; }

        public int TicketNo { get; set; }

        public decimal TotalTurnoverHome { get; set; }

        public decimal TotalTurnoverAway { get; set; }

        public decimal TotalTurnoverOver { get; set; }

        public decimal TotalTurnoverUnder { get; set; }
    }
}