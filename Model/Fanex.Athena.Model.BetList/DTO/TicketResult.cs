// -----------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.DTO
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TicketResult class.
    /// </summary>
    public class TicketResult
    {
        private SortableList<LrfTicket> _mainTicketList = new SortableList<LrfTicket>();
        private List<ITicketData> _ticketDataList = new List<ITicketData>();

        [System.CLSCompliant(false)]
        public SortableList<LrfTicket> MainTicketList
        {
            get
            {
                return _mainTicketList;
            }
        }

        [System.CLSCompliant(false)]
        public List<ITicketData> TicketDataList
        {
            get
            {
                return _ticketDataList;
            }
        }

        // Sum value
        public int TotalRows { get; set; }

        public decimal SumWinLoss { get; set; }

        public decimal SumBetEffective { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }
    }
}