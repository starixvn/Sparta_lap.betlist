namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    #region Using

    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ResultModel;

    #endregion Using

    public interface ICustomerListDataAccess
    {
        #region Read

        IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml);

        ReportResultModel<CustomerDTO> Search(CustomerListMetaModel metaModel);

        #endregion Read

        #region LogInfo

        LogInfo GetCustomerLoginInfo(int custId, int licenseeId);

        #endregion LogInfo

        #region Group Commission

        IResultInfo MoveCustomersToGroup(MoveToGroupComMetaModel metaModel);

        IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel);

        #endregion Group Commission

        #region Update

        IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel);

        IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel);

        IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request);

        IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel);

        IResultInfo UpdateSpecialLevel(int userId, int custId, int level, string ip, string language);

        IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel);

        #endregion Update
    }
}