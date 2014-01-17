// -----------------------------------------------------------------------
// <copyright file="IDailyRacingWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IDailyRacingWinLossService
    {
        #region Public Methods and Operators

        IList<DailyRacingWinLoss> GetDataList(DailyRacingWinLossMetaModel metaModel);

        DailyRacingWinLossData GetDailyRacingWinLossData(DailyRacingWinLossMetaModel metaModel);

        #endregion Public Methods and Operators
    }
}