// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;

    /// <summary>
    /// MicroGamingWinLoss Entity Class.
    /// </summary>
    public class MicroGamingWinLoss
    {
        public int RowSpan { get; set; }

        public int Index { get; set; }

        public int TypeID { get; set; }

        public string TypeName { get; set; }

        public DateTime Date { get; set; }

        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public decimal TurnOver { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal ProgressiveContribution { get; set; }

        public decimal ProgressivePayout { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal LicenseeNetWin { get; set; }

        public decimal AmountDueToMGC { get; set; }

        public decimal ProgresssiveFee { get; set; }
    }
}