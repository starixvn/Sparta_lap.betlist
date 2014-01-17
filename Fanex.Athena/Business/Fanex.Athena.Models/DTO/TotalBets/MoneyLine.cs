// -----------------------------------------------------------------------
// <copyright file="MoneyLine.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
{
    /// <summary>
    /// MoneyLine class.
    /// </summary>
    public class MoneyLine : TotalBetsBase
    {
        public decimal TotalHomeFt { get; set; }

        public decimal TotalAwayFt { get; set; }

        public decimal ForecastHomeFt { get; set; }

        public decimal ForecastAwayFt { get; set; }

        public decimal TotalHomeFirstHalf { get; set; }

        public decimal TotalAwayFirstHalf { get; set; }

        public decimal ForecastHomeFirstHalf { get; set; }

        public decimal ForecastAwayFirstHalf { get; set; }
    }
}