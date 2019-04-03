namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustomerListDataAccess
    {
        Task<ReportResultModel<CustomerDTO>> GetActiveCustomers(CustomerListMetaModel metaModel);

        #region LogInfo

        LogInfo GetCustomerLoginInfo(int custId, int licenseeId, string siteName);

        #endregion LogInfo

        #region Update

        IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel);

        IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel);

        IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request);

        IResultInfo UpdateSpecialLevel(int userId, int custId, int level, string ip, string language);

        IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel);

        #endregion Update
    }
}