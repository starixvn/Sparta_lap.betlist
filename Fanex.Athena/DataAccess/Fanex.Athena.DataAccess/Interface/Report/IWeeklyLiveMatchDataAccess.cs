// -----------------------------------------------------------------------
// <copyright file="IWeeklyLiveMatchDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IWeeklyLiveMatchDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WeeklyLiveMatch> GetDataList(WeeklyLiveMatchMetaModel metaModel, out WeeklyLiveMatch total);
        
        #endregion Public Methods and Operators
    }
}