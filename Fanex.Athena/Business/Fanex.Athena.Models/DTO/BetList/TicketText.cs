// -----------------------------------------------------------------------
// <copyright file="TicketText.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetList
{
    /// <summary>
    /// TicketText class.
    /// </summary>
    public class TicketText
    {
        public int Index { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public string TransTime { get; set; }

        public string Choice { get; set; }

        public string Odds { get; set; }

        public string Stake { get; set; }

        public string WinLossComm { get; set; }

        public string LicenseeWl { get; set; }

        public string Status { get; set; }
    }
}