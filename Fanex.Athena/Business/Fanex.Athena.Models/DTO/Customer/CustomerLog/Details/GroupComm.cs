// -----------------------------------------------------------------------
// <copyright file="GroupComm.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.CustomerLog.Details
{
    /// <summary>
    /// GroupCommission class.
    /// </summary>
    public class GroupComm : MemberComm
    {
        public string GroupName { get; set; }

        public string GroupNameOld { get; set; }

        public string GroupColor { get; set; }

        public string GroupColorOld { get; set; }
    }
}