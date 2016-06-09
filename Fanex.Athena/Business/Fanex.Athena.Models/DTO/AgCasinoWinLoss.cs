// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    /// <summary>
    /// AgCasinoWinLoss class.
    /// </summary>
    public class AgCasinoWinLoss
    {
        public int RowSpan { get; set; }

        public int Index { get; set; }

        public DateTime? Date { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public decimal TurnOver { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal LicenseeWL { get; set; }
    }
}