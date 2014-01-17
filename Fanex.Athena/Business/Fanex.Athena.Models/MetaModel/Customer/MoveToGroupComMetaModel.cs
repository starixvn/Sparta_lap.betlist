// -----------------------------------------------------------------------
// <copyright file="MoveToGroupComMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    /// <summary>
    /// MoveToGroupComMetaModel class.
    /// </summary>
    public class MoveToGroupComMetaModel : BaseCustomerComMetaModel
    {
        public int GroupComId { get; set; }

        public bool IsAllowedNG { get; set; }

        public bool IsAllowedRC { get; set; }

        public bool IsAllowedLC { get; set; }

        public bool IsAllowedRNG { get; set; }
    }
}