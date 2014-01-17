// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWinLossService.cs" company="Nexcel">
// The  win loss service.
// </copyright>
// <summary>
//   The  win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;

    /// <summary>
    ///     The i win loss service.
    /// </summary>
    public interface IWinLossService
    {
        #region Public Methods and Operators

        DataTable GetDataForExcel(WinLossMetaModel model, string licenseeList);

        IList<WinLoss> GetDataList(
            WinLossMetaModel model,
            string licenseeList,
            ref IDictionary<string, decimal> totalValues);

        Dictionary<string, decimal> GetWinlossTotalInfo(IList<WinLoss> dataList);

        #endregion Public Methods and Operators
    }
}