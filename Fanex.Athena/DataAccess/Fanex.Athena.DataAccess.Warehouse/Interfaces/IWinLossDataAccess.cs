// -----------------------------------------------------------------------
// <copyright file="IWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Warehouse.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
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