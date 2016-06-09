// -----------------------------------------------------------------------
// <copyright file="ILiveReportDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    /// Live report data access interface.
    /// </summary>
    public interface ILiveReportDataAccess
    {
        IList<LiveReport> GetLiveReportSummary(LiveReportMetalModel metaModel, DateTime moveBetsDate);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        void GetLiveReportSummaryDetail(
            LiveReportMetalModel metaModel,
            DateTime moveBetsDate,
            string productCode,
            IList<Product> products,
            out IList<LiveReport> resultBySport,
            out IList<LiveReport> resultByByCurrency,
            out IList<LiveReportByBetType> resultByBettype);

        IList<LiveReportWinLossByMatch> GetLiveReportWinlossByMatch(LiveReportMatchListMetalModel metaModel, DateTime moveBetsDate);
    }
}