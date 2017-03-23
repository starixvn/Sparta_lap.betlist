// -----------------------------------------------------------------------
// <copyright file="CustomerLogService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;
    using Fanex.Athena.Service.Customer.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerLogService : ICustomerLogService
    {
        #region Field

        private readonly ICustomerLogDataAccess _customerLogDataAccess;

        #endregion Field

        #region Constructor

        public CustomerLogService()
        {
            this._customerLogDataAccess = new CustomerLogDataAccess();
        }

        public CustomerLogService(ICustomerLogDataAccess customerLogDataAccess)
        {
            this._customerLogDataAccess = customerLogDataAccess;
        }

        #endregion Constructor

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