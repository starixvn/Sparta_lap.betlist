// -----------------------------------------------------------------------
// <copyright file="IDailyRacingWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IDailyRacingWinLossDataAccess
    {
        #region Public Methods and Operators

        IList<DailyRacingWinLoss> GetDataList(DailyRacingWinLossMetaModel metaModel);

        IList<DailyRacingWinLoss> SetToList(IDataReader reader, int licenseeId, string baseCurrency);

        #endregion Public Methods and Operators
    }
}