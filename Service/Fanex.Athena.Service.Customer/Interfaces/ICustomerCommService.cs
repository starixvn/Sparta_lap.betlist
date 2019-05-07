namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;

    public interface ICustomerCommService
    {
        /// <summary>
        ///     Move customers to new group including 2 following steps:
        ///     1: Remove all customers commission in group.
        ///     2: Update new customers commission in this group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The result.</returns>
        Task<IResultInfo> MoveCustomersToGroupAsync(MoveToGroupComMetaModel metaModel);

        Task<IResultInfo> RemoveCustomersFromGroupAsync(RemoveFromGroupComMetaModel metaModel, bool isAllowedRNGCasino);

        Task<IResultInfo> UpdateCustomersCommissionAsync(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false);
    }
}