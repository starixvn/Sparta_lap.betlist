// -----------------------------------------------------------------------
// <copyright file="ICustomerDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    #region Using

    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;

    #endregion Using

    /// <summary>
    /// Interface ICustomerDataAccess contains all method relates to customer.
    /// </summary>
    public interface ICustomerDataAccess
    {
        #region Read

        IEnumerable<string> SearchCustomers(int userId, string userName, int licenseeId, string licenseeType, int currencyId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<CustomerDTO> Search(CustomerListMetaModel metaModel, ref int totalRows);

        CustomerInfo GetCustomerInfo(int custId);

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

        #endregion Update
    }
}