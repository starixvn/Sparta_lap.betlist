// -----------------------------------------------------------------------
// <copyright file="UpdatedCustomerComMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    /// <summary>
    /// UpdatedCustomerComMetaModel class.
    /// </summary>
    public class UpdatedCustomerComMetaModel : BaseCustomerComMetaModel
    {
        public int GroupId { get; set; }

        public bool IsAllowedNG { get; set; }

        public bool IsAllowedRC { get; set; }

        public bool IsAllowedLC { get; set; }

        public bool IsAllowedCS { get; set; }

        public decimal SBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal RCComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal CSComm { get; set; }
    }
}