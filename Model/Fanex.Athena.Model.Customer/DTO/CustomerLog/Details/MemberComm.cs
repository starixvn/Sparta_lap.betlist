// -----------------------------------------------------------------------
// <copyright file="MemberComm.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.DTO.CustomerLog.Details
{
    /// <summary>
    /// MemberCommission class.
    /// </summary>
    public class MemberComm
    {
        public int GroupId { get; set; }

        public int GroupIdOld { get; set; }

        public decimal SBComm { get; set; }

        public decimal RBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal SBCommOld { get; set; }

        public decimal RBCommOld { get; set; }

        public decimal NGCommOld { get; set; }
    }
}