// -----------------------------------------------------------------------
// <copyright file="CustomerDTO.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer
{
    /// <summary>
    /// CustomerDTO class.
    /// </summary>
    public class CustomerDTO
    {
        public int Index { get; set; }

        public int CustId { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public decimal AvailableFunds { get; set; }

        public decimal PendingFunds { get; set; }

        public string Status { get; set; }

        public bool IsClosed { get; set; }

        public bool IsSuspended { get; set; }

        public bool BaStatus { get; set; }

        public int SpecialLevel { get; set; }

        public int GroupCommId { get; set; }

        public string Group { get; set; }

        public string GroupColor { get; set; }

        public decimal SbComm { get; set; }

        public decimal RbComm { get; set; }

        public decimal NgComm { get; set; }

        public decimal LcComm { get; set; }

        public decimal CsComm { get; set; }

        public bool IsPowerBAUserTurnoff { get; set; }
    }
}