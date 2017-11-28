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
        SortableReportResultModel<ColossusBetWinLoss> GetWinlossDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<ColossusBetWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel);
    }
}