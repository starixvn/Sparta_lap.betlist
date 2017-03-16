// -----------------------------------------------------------------------
// <copyright file="IAgCasinoBalanceDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
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
    public interface IAgCasinoBalanceDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<AgCasinoBalance> GetAgentWinlossDetail(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<AgCasinoBalance> GetCustomerWinlossDetail(
            AgCasinoBalanceMetaModel metaModel, 
            out IDictionary<string, AgCasinoBalance> summaryTotal, 
            out int totalRows);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<AgCasinoBalance> GetSummaryWinloss(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal);

        #endregion Public Methods and Operators
    }
}