// -----------------------------------------------------------------------
// <copyright file="PlaytechCasino.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    /// <summary>
    /// PLAYTECH Casino Item.
    /// </summary>
    public class PlaytechCasino
    {
        public DateTime WinlossDate { get; set; }

        public int CustId { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public decimal Turnover { get; set; }

        public int TotalTrans { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get
            {
                return CustomerWinLoss + CustomerComm;
            }
        }

        public decimal LicenseeWinLoss { get; set; }

        public decimal ProgressiveShare { get; set; }

        public decimal ProgressiveWin { get; set; }

        public decimal NetGaming { get; set; }
    }
}
