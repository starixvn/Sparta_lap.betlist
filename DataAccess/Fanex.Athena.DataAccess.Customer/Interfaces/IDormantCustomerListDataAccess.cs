namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ResultModel;

    public interface IDormantCustomerListDataAccess
    {
        ReportResultModel<DormantCustomer> GetDormantCustomers(CustomerListMetaModel metaModel);
    }
}