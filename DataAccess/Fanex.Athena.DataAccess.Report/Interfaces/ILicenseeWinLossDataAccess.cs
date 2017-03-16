// -----------------------------------------------------------------------
// <copyright file="ILicenseeWinLossDataAccess.cs" company="">
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
    ///     The licensee win loss data access.
    /// </summary>
    public interface ILicenseeWinLossDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<LicenseeWinLoss> GetAgentWinlossDetail(LicenseeWinLossMetaModel metaModel, out IDictionary<string, LicenseeWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<LicenseeWinLoss> GetCustomerWinlossDetail(LicenseeWinLossMetaModel metaModel, out IDictionary<string, LicenseeWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<LicenseeWinLoss> GetSummaryWinloss(LicenseeWinLossMetaModel metaModel, out IDictionary<string, LicenseeWinLoss> summaryTotal);

        #endregion Public Methods and Operators
    }
}