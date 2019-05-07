namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ResultModel;

    public interface IDormantCustomerListDataAccess
    {
        Task<ReportResultModel<DormantCustomer>> GetDormantCustomers(CustomerListMetaModel metaModel);
    }
}