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

    #endregion Using

    /// <summary>
    /// Interface ICustomerService contains all method relates to customer.
    /// </summary>
    public interface ICustomerService
    {
        #region Read

        /// <summary>
        /// Quick search by userName.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="licenseeType">The licensee type.</param>
        /// <param name="currencyId">The currency id.</param>
        /// <returns>The data table.</returns>
        IEnumerable<string> SearchCustomers(int userId, string userName, int licenseeId, string licenseeType, int currencyId);

        /// <summary>
        /// Advance search customer.
        /// </summary>
        /// <param name="metaModel">Meta model contains the filter such as: BA type, special level type.</param>
        /// <param name="totalRows">The total rows.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<CustomerDTO> Search(CustomerListMetaModel metaModel, ref int totalRows);

        /// <summary>
        /// Get customer Information such as: SyncCasino, DisableBingo, ...
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <returns>The customer info.</returns>
        CustomerInfo GetCustomerInfo(int licenseeId, int custId);

        #endregion Read

        #region LogInfo

        /// <summary>
        /// Get customer login information, last transfer time, ...
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <returns>The login info.</returns>
        LogInfo GetCustomerLoginInfo(int custId, int licenseeId);

        #endregion LogInfo

        #region Group Commission

        /// <summary>
        ///     Move customers to new group including 2 following steps:
        ///     1: Remove all customers commission in group.
        ///     2: Update new customers commission in this group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The result.</returns>
        IResultInfo MoveCustomersToGroup(MoveToGroupComMetaModel metaModel);

        /// <summary>
        /// Remove customer from commission group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="isAllowedRNGCasino">Allow RNG casino or not.</param>
        /// <returns>The result.</returns>
        IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel, bool isAllowedRNGCasino);

        #endregion Group Commission

        IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel);

        IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode);

        IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode);

        IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request);

        IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false);

        IResultInfo UpdateSpecialLevel(int licenseeId, int userId, int custId, int level, string ip, string language);
    }
}