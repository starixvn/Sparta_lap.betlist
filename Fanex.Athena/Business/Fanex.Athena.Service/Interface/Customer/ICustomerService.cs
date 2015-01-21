// -----------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Customer
{
    #region Using

    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.Customer;

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
        DataTable QuickSearch(int userId, string userName, int licenseeId, string licenseeType, int currencyId);

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
        CustomerInfo GetCustomerInfo(int custId);

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

        #region Update        
        
        /// <summary>
        /// Update customer odds group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The result information.</returns>
        IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel);

        /// <summary>
        /// Update customer closed status.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The result.</returns>
        IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode);

        /// <summary>
        /// Update customer suspend status.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The result.</returns>
        IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode);

        /// <summary>
        /// Update customer BA status.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="modifiedBy">The user who updated customer.</param>
        /// <param name="allowBA">Allowable BA.</param>
        /// <param name="request">The request info.</param>
        /// <returns>The result.</returns>
        IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request);

        /// <summary>
        ///  Update customer commission setting.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="isSyncCSComm">Sync RNG commission or not.</param>
        /// <returns>The result.</returns>
        IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false);

        /// <summary>
        /// Update customer special level.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="level">The level of customer.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="language">The language code.</param>
        /// <returns>The result.</returns>
        IResultInfo UpdateSpecialLevel(int userId, int custId, int level, string ip, string language);

        #endregion Update
    }
}