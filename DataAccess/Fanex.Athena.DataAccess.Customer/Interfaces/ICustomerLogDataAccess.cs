// -----------------------------------------------------------------------
// <copyright file="ICustomerLogDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerLogDataAccess
    {
        IList<CustomerLog> GetCustomerSettingList(CustomerLogMetaModel metaModel);

        IList<CustomerLog> GetGroupCommList(CustomerLogMetaModel metaModel);
    }
}