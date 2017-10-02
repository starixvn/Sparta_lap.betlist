namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;

    public interface ICustomerCommDataAccess
    {
        IResultInfo MoveCustomersToGroup(MoveToGroupComMetaModel metaModel);

        IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel);

        IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel);
    }
}