// -----------------------------------------------------------------------
// <copyright file="IWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;

    /// <summary>
    ///     The i win loss data access.
    /// </summary>
    public interface IWinLossDataAccess
    {
        #region Public Methods and Operators

        DataTable GetDataForExcel(WinLossMetaModel model, string licenseeList);

        IList<WinLoss> GetDataList(WinLossMetaModel model, string licenseeList);

        #endregion Public Methods and Operators
    }
}