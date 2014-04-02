// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITopCustomerDataAccess.cs" company="Nexcel">
// a
// </copyright>
// <summary>
//   The i top customer data access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;

    /// <summary>
    /// The i top customer data access.
    /// </summary>
    public interface ITopCustomerDataAccess
    {
        #region Public Methods and Operators

        DataTable GetDataForExcel(TopCustomerMetaModel model);

        IList<TopCustomer> GetDataList(TopCustomerMetaModel model);

        #endregion Public Methods and Operators
    }
}