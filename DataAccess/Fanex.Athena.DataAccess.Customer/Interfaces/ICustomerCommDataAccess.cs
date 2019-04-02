namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;

    public interface ICustomerCommDataAccess
    {
        Task<IResultInfo> MoveCustomersToGroupAsync(MoveToGroupComMetaModel metaModel);

        Task<IResultInfo> RemoveCustomersFromGroupAsync(RemoveFromGroupComMetaModel metaModel);

        Task<IResultInfo> UpdateCustomersCommissionAsync(UpdatedCustomerComMetaModel metaModel);
    }
}