// -----------------------------------------------------------------------
// <copyright file="LrfTicket.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.DTO
{
    using Fanex.BetList.Core.Entities;

#pragma warning disable CS3009 // Base type is not CLS-compliant

    public class LrfTicket : Ticket
#pragma warning restore CS3009 // Base type is not CLS-compliant
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
                return Winlost + PlayerComm;
            }
        }

        public decimal LicenseeWLComm
        {
            get
            {
                return LicenseeWL + Comm;
            }
        }
    }
}