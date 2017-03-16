// -----------------------------------------------------------------------
// <copyright file="IWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Warehouse.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;

    /// <summary>
    ///     The i win loss data access.
    /// </summary>
    public interface IWinLossDataAccess
    {
        #region Public Methods and Operators

        IList<WinLoss> GetDataList(WinLossMetaModel metaModel);

        #endregion Public Methods and Operators
    }
}