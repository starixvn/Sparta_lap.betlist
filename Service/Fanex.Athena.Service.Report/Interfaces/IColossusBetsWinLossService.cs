// -----------------------------------------------------------------------
// <copyright file="IColossusBetsWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IColossusBetsWinLossService
    {
        SortableReportResultModel<ColossusBetWinLoss> GetWinlossDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<ColossusBetWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel);

        IEnumerable<ColossusBetWinLoss> CalculateTotal(IEnumerable<ColossusBetWinLoss> dataList);
    }
}