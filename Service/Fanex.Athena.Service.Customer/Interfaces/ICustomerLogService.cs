// -----------------------------------------------------------------------
// <copyright file="ICustomerLogService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerLogService
    {
        IList<CustomerLog> GetCustomerSettingList(CustomerLogMetaModel metaModel);

        IList<CustomerLog> GetGroupCommList(CustomerLogMetaModel metaModel);
    }
}