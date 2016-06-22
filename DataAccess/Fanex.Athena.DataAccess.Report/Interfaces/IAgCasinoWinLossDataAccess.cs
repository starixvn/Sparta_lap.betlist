// -----------------------------------------------------------------------
// <copyright file="IAgCasinoWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     The Ag casino win loss data access.
    /// </summary>
    public interface IAgCasinoWinLossDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<AgCasinoWinLoss> GetAgentWinlossDetail(AgCasinoWinLossMetaModel metaModel, out IDictionary<string, AgCasinoWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<AgCasinoWinLoss> GetCustomerWinlossDetail(AgCasinoWinLossMetaModel metaModel, out IDictionary<string, AgCasinoWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel, out IDictionary<string, AgCasinoWinLoss> summaryTotal);

        #endregion Public Methods and Operators
    }
}