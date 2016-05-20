// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWinLossService.cs" company="Nexcel">
// The  win loss service.
// </copyright>
// <summary>
//   The  win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Warehouse.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;

    /// <summary>
    ///     The i win loss service.
    /// </summary>
    public interface IWinLossService
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        IList<WinLoss> GetDataList(WinLossMetaModel model, ref IDictionary<string, decimal> totalValues);

        Dictionary<string, decimal> GetWinlossTotalInfo(IList<WinLoss> dataList);

        #endregion Public Methods and Operators
    }
}