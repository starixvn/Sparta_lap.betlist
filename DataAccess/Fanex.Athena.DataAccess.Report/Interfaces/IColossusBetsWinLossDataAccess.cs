// -----------------------------------------------------------------------
// <copyright file="IColossusBetsWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IColossusBetsWinLossDataAccess
    {
        SortableReportResultModel<ColossusBetWinLoss> GetWinlossDetail(ColossusBetsWinLossMetaModel metaModel);

        SortableReportResultModel<ColossusBetWinLoss> GetSummaryWinloss(ColossusBetsWinLossMetaModel metaModel);
    }
}