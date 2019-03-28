// -----------------------------------------------------------------------
// <copyright file="ILiveReportDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ILiveReportDataAccess
    {
        Task<IList<LiveReport>> GetLiveReportSummary(LiveReportMetaModel metaModel);

        Task<IList<LiveReport>> GetDetailBySport(LiveReportMetaModel metaModel);

        Task<Tuple<IList<LiveReport>, IList<LiveReportByBetType>>> GetDetailByCurrencyAndBettype(
            LiveReportMetaModel metaModel,
                        IDictionary<int, string> currencyList,
            IEnumerable<Product> products);

        Task<IList<LiveReportWinLossByMatch>> GetLiveReportWinlossByMatch(LiveReportMatchListMetalModel metaModel);
    }
}