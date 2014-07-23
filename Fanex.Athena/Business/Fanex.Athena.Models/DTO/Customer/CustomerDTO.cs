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

        public bool BAStatus { get; set; }

        public int SpecialLevel { get; set; }

        public int GroupCommId { get; set; }

        public string Group { get; set; }

        public string GroupColor { get; set; }

        public decimal SBComm { get; set; }

        public decimal RBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal CSComm { get; set; }

        public bool IsPowerBAUserTurnoff { get; set; }
    }
}