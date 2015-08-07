// -----------------------------------------------------------------------
// <copyright file="IMicroGamingWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMicroGamingWinLossDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel, out IDictionary<string, MicroGamingWinLoss> summaryTotal);

        #endregion Public Methods and Operators
    }
}
