// -----------------------------------------------------------------------
// <copyright file="ICashOutWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICashOutWinLossDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetAgentWinlossDetail(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetCustomerWinlossDetail(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetSummaryWinloss(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);
    }
}