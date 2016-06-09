// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuTotalItem.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.DTO.NewForecastHdpOu
{
    /// <summary>
    /// NewForecast HDP OU TotalItem class.
    /// </summary>
    public class NewForecastHdpOuTotalItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Reviewed.")]
        public int Type { get; set; }

        public int TicketNo { get; set; }

        public decimal TotalTurnoverHome { get; set; }

        public decimal TotalTurnoverAway { get; set; }

        public decimal TotalTurnoverOver { get; set; }

        public decimal TotalTurnoverUnder { get; set; }
    }
}