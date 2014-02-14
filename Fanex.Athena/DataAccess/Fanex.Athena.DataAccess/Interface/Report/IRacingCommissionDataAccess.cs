// -----------------------------------------------------------------------
// <copyright file="IRacingCommissionDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Data;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IRacingCommissionDataAccess
    {
        #region Public Methods and Operators

        SortableList<RacingCommission> GetDataList(RacingCommissionMetaModel metaModel, ref RacingCommission total);

        SortableList<RacingCommission> SetToList(IDataReader reader, string baseCurrency, ref RacingCommission total);

        #endregion Public Methods and Operators
    }
}