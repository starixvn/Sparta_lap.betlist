// -----------------------------------------------------------------------
// <copyright file="IWeeklyLiveMatchDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IWeeklyLiveMatchDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WeeklyLiveMatch> GetDataList(WeeklyLiveMatchMetaModel metaModel, out int[] total);
    }
}