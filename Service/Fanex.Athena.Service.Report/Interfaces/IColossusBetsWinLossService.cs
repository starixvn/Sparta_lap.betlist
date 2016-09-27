// -----------------------------------------------------------------------
// <copyright file="IColossusBetsWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IColossusBetsWinLossService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<ColossusBetWinLoss> GetWinlossDetail(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ColossusBetWinLoss> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<ColossusBetWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ColossusBetWinLoss> summaryTotal);

        IList<ColossusBetWinLoss> TotalCalc(IList<ColossusBetWinLoss> dataList);
    }
}