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

        DataTable GetDataForExcel(TopCustomerMetaModel model);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        IList<TopCustomer> GetTopCustomerList(
            TopCustomerMetaModel model, ref Dictionary<string, decimal> totalValues);

        Dictionary<string, decimal> GetTopCustomerTotalInfo(IList<TopCustomer> dataList);

        #endregion Public Methods and Operators
    }
}