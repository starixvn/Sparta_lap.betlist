// -----------------------------------------------------------------------
// <copyright file="ICustomerLogService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Models.MetaModel.Customer.CustomerLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerLogService
    {
        IList<CustomerLog> GetCustomerSettingList(CustomerLogMetaModel metaModel);

        IList<CustomerLog> GetGroupCommList(CustomerLogMetaModel metaModel);
    }
}