// -----------------------------------------------------------------------
// <copyright file="ILiveReportDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ILiveReportDataAccess
    {
        IList<LiveReport> GetLiveReportSummary(LiveReportMetaModel metaModel);

        IList<LiveReport> GetDetailBySport(LiveReportMetaModel metaModel);

        Tuple<IList<LiveReport>, IList<LiveReportByBetType>> GetDetailByCurrencyAndBettype(
            LiveReportMetaModel metaModel,
            IEnumerable<Product> products);

        IList<LiveReportWinLossByMatch> GetLiveReportWinlossByMatch(LiveReportMatchListMetalModel metaModel);
    }
}