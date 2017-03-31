// -----------------------------------------------------------------------
// <copyright file="CashOutWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class CashOutWinLoss
    {
        public int RowSpan { get; set; }

        public int Index { get; set; }

        public DateTime Date { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public decimal CashOutTimes { get; set; }

        public decimal CashOutTickets { get; set; }

        public decimal CashOutValue { get; set; }

        public decimal BuyBackAmountSB { get; set; }

        public decimal BuyBackAmountBA { get; set; }

        public decimal TotalBuyBackAmount
        {
            get
            {
                return BuyBackAmountSB + BuyBackAmountBA;
            }
        }

        public decimal TicketPayout { get; set; }

        public decimal CashOutCommission { get; set; }

        public decimal SettleAmountTicketPayout { get; set; }

        public decimal SettleAmountBuyBackComm { get; set; }

        public decimal SettleAmountTotal
        {
            get
            {
                return SettleAmountTicketPayout + SettleAmountBuyBackComm;
            }
        }
    }
}