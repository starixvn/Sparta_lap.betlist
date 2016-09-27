// -----------------------------------------------------------------------
// <copyright file="ColossusBetWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class ColossusBetWinLoss
    {
        public int RowSpan { get; set; }

        public int Index { get; set; }

        public DateTime Date { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public decimal TurnOver { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal WinningPrize { get; set; }

        public decimal CashOut { get; set; }

        public decimal CashIn { get; set; }

        public decimal CommissionShare { get; set; }

        public decimal AmountDueToCB
        {
            get
            {
                return TurnOver - WinningPrize - CashOut - CashIn - CommissionShare;
            }
        }

        public decimal GrandPrize { get; set; }
    }
}