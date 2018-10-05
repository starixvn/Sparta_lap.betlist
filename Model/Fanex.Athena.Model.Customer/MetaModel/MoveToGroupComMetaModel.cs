// -----------------------------------------------------------------------
// <copyright file="MoveToGroupComMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.Athena.Model.Core.DTO.Common;

namespace Fanex.Athena.Model.Customer.MetaModel
{
    /// <summary>
    /// MoveToGroupComMetaModel class.
    /// </summary>
    public class MoveToGroupComMetaModel : BaseCustomerComMetaModel
    {
        public MoveToGroupComMetaModel()
        {
        }

        public MoveToGroupComMetaModel(ProductAccess productAccess)
        {
            IsAllowedNG = productAccess.IsAllowNG;
            IsAllowedRNG = productAccess.IsAllowCS;
        }

        public int GroupComId { get; set; }

        public bool IsAllowedNG { get; set; }

        public bool IsAllowedRNG { get; set; }
    }
}