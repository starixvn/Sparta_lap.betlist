// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICashOutWinLossService.cs" company="">
// The i licensee win loss service.
// </copyright>
// <summary>
//   The i licensee win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICashOutWinLossService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetDetail(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetSummary(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        IList<CashOutWinLoss> TotalCalc(IList<CashOutWinLoss> dataList);
    }
}