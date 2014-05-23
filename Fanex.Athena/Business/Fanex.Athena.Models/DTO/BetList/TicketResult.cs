// -----------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetList
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TicketResult class.
    /// </summary>
    public class TicketResult
    {
        public TicketResult()
        {
            MainTicketList = new SortableList<LrfTicket>();
            TicketDataList = new List<ITicketData>();
        }

        public SortableList<LrfTicket> MainTicketList { get; set; }

        public List<ITicketData> TicketDataList { get; set; }

        // Sum value
        public int TotalRows { get; set; }

        public decimal SumWinLoss { get; set; }

        public decimal SumLicenseeWl { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public decimal SumStake { get; set; }
    }
}