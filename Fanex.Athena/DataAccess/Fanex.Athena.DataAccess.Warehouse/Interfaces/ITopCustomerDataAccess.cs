// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITopCustomerDataAccess.cs" company="Nexcel">
// a
// </copyright>
// <summary>
//   The i top customer data access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Warehouse.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;

    /// <summary>
    /// The i top customer data access.
    /// </summary>
    public interface ITopCustomerDataAccess
    {
        #region Public Methods and Operators

        DataTable GetDataForExcel(TopCustomerMetaModel metaModel);

        IList<TopCustomer> GetDataList(TopCustomerMetaModel metaModel);

        #endregion Public Methods and Operators
    }
}