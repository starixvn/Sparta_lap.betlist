// -----------------------------------------------------------------------
// <copyright file="LrfTicket.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetList
{
    using System;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// LRFTicket class.
    /// </summary>
    [System.CLSCompliant(false)]
    public class LrfTicket : Ticket
    {
        public string ExternalId { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal BetEffective { get; set; } 

        public int CurrencyId { get; set; }

        public int SpecialLevel { get; set; }

        public string BetFrom { get; set; }

        public decimal WinLossComm 
        {
            get
            {
                return Winlost + Comm;
            }        
        }
    }
}