// -----------------------------------------------------------------------
// <copyright file="CustomerLogService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Customer.CustomerLog
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Customer.CustomerLog;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Models.MetaModel.Customer.CustomerLog;
    using Fanex.Athena.Service.Interface.Customer.CustomerLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerLogService : ICustomerLogService
    {
        #region Field

        private readonly CustomerLogDataAccess _customerLogDataAccess;

        #endregion Field

        #region Constructor

        public CustomerLogService()
        {
            this._customerLogDataAccess = new CustomerLogDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Get customer setting data list.
        /// </summary>
        /// <param name="metaModel">Customer log parameter.</param>
        /// <returns>The customer log list.</returns>
        public IList<CustomerLog> GetCustomerSettingList(CustomerLogMetaModel metaModel)
        {
            var dataList = this._customerLogDataAccess.GetCustomerSettingList(metaModel).OrderByDescending(x => x.LogId).ToList();

            return dataList;
        }

        /// <summary>
        /// Get group commission data list.
        /// </summary>
        /// <param name="metaModel">Customer log parameter.</param>
        /// <returns>The customer-log list.</returns>
        public IList<CustomerLog> GetGroupCommList(CustomerLogMetaModel metaModel)
        {
            var dataList = this._customerLogDataAccess.GetGroupCommList(metaModel);

            return dataList;
        }
    }
}