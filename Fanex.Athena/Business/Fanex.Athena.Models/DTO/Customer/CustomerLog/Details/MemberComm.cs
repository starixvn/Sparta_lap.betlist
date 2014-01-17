// -----------------------------------------------------------------------
// <copyright file="MemberComm.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.CustomerLog.Details
{
    /// <summary>
    /// MemberCommission class.
    /// </summary>
    public class MemberComm
    {
        public int GroupId { get; set; }

        public int GroupIdOld { get; set; }

        public decimal SbComm { get; set; }

        public decimal RbComm { get; set; }

        public decimal NgComm { get; set; }

        public decimal SbCommOld { get; set; }

        public decimal RbCommOld { get; set; }

        public decimal NgCommOld { get; set; }
    }
}