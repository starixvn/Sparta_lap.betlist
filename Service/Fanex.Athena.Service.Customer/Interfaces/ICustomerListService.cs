// -----------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    #region Using

    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ResultModel;

    #endregion Using

    public interface ICustomerListService
    {
        IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml);

        ReportResultModel<CustomerDTO> GetActiveCustomers(CustomerListMetaModel metaModel);

        ReportResultModel<DormantCustomer> GetDormantCustomers(CustomerListMetaModel metaModel);

        #region LogInfo

        /// <summary>
        /// Get customer login information, last transfer time, ...
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <returns>The login info.</returns>
        LogInfo GetCustomerLoginInfo(int custId, int licenseeId, string siteName);

        #endregion LogInfo

        IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel);

        IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode);

        IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode);

        IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request);

        IResultInfo UpdateSpecialLevel(int licenseeId, int userId, int custId, int level, string ip, string language);
    }
}