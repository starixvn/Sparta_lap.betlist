// -----------------------------------------------------------------------
// <copyright file="IGoldDeluxeWinLossDataAccess.cs" company="Nexcel Solution">
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
    ///     The Gold deluxe win loss data access.
    /// </summary>
    public interface IGoldDeluxeWinLossDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<GoldDeluxeWinLoss> GetAgentWinlossDetail(GoldDeluxeWinLossMetaModel metaModel, out IDictionary<string, GoldDeluxeWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<GoldDeluxeWinLoss> GetCustomerWinlossDetail(GoldDeluxeWinLossMetaModel metaModel, out IDictionary<string, GoldDeluxeWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<GoldDeluxeWinLoss> GetSummaryWinloss(GoldDeluxeWinLossMetaModel metaModel, out IDictionary<string, GoldDeluxeWinLoss> summaryTotal);

        #endregion Public Methods and Operators
    }
}