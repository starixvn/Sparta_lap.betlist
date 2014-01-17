// -----------------------------------------------------------------------
// <copyright file="ICustomerLogDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Customer.CustomerLog
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Models.MetaModel.Customer.CustomerLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerLogDataAccess
    {
        IList<CustomerLog> GetCustomerSettingList(CustomerLogMetaModel metaModel);

        IList<CustomerLog> GetGroupCommList(CustomerLogMetaModel metaModel);
    }
}