// -----------------------------------------------------------------------
// <copyright file="PlaytechCasino.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;

    /// <summary>
    /// Playtech Casino Item.
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
    }
}
