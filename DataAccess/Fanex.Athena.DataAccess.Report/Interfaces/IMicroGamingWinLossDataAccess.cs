// -----------------------------------------------------------------------
// <copyright file="IMicroGamingWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IMicroGamingWinLossDataAccess
    {
        SortableSummaryReportResultModel<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel);
    }
}