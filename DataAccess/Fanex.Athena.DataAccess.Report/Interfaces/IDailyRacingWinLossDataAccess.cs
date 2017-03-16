// -----------------------------------------------------------------------
// <copyright file="IDailyRacingWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

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