// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITopCustomerService.cs" company="Nexel">
// The  top customer service.
// </copyright>
// <summary>
//   The  top customer service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;

    /// <summary>
    ///     The i top customer service.
    /// </summary>
    public interface ITopCustomerService
    {
        #region Public Methods and Operators

        DataTable GetDataForExcel(TopCustomerMetaModel model, string licenseeList);

        IList<TopCustomer> GetTopCustomerList(
            TopCustomerMetaModel model, string licenseeList, ref Dictionary<string, decimal> totalValues);

        Dictionary<string, decimal> GetTopCustomerTotalInfo(IList<TopCustomer> dataList);

        #endregion Public Methods and Operators
    }
}