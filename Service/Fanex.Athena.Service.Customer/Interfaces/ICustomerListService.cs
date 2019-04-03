// -----------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Threading.Tasks;

    #region Using

    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ResultModel;

    #endregion Using

    public interface ICustomerListService
    {
        Task<ReportResultModel<CustomerDTO>> GetActiveCustomers(CustomerListMetaModel metaModel);

        Task<ReportResultModel<DormantCustomer>> GetDormantCustomers(CustomerListMetaModel metaModel);

        Task<LogInfo> GetCustomerLoginInfo(int custId, int licenseeId, string siteName);

        Task<IResultInfo> UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel);

        Task<IResultInfo> UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode);

        Task<IResultInfo> UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode);

        Task<IResultInfo> UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request);

        Task<IResultInfo> UpdateSpecialLevel(int licenseeId, int userId, int custId, int level, string ip, string language);
    }
}