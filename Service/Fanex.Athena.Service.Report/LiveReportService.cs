// -----------------------------------------------------------------------
// <copyright file="LiveReportService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    /// <summary>
    /// Live report service class.
    /// </summary>
    public class LiveReportService : ILiveReportService
    {
        private readonly IBalanceUpService _balanceUpService;
        private readonly ILiveReportDataAccess _liveReportDataAccess;

        public LiveReportService()
        {
            this._liveReportDataAccess = new LiveReportDataAccess();
            this._balanceUpService = new BalanceUpService();
        }

        /// <summary>
        /// Get summary data of live report.
        /// </summary>
        /// <param name="model">The metal model.</param>
        /// <returns>The summary data list.</returns>
        public IList<LiveReport> GetLiveReportSummary(LiveReportMetalModel model)
        {
            if (model.IsValid)
            {
                var balance = this._balanceUpService.BalanceUpInfo;
                return this._liveReportDataAccess.GetLiveReportSummary(model, balance.MoveBetsDay);
            }

            return new List<LiveReport>();
        }

        /// <summary>
        /// Get detail data of live report.
        /// </summary>
        /// <param name="model">The metal model.</param>
        /// <param name="productCode">The product code.</param>
        /// <param name="products">The products.</param>
        /// <param name="resultBySport">The result data group by sport.</param>
        /// <param name="resultByByCurrency">The result data group by currency.</param>
        /// <param name="resultByBettype">The result data group by bet type.</param>
        public void GetLiveReportSumaryDetail(
            LiveReportMetalModel model,
            string productCode,
            IList<Product> products,
            out IList<LiveReport> resultBySport,
            out IList<LiveReport> resultByByCurrency,
            out IList<LiveReportByBetType> resultByBettype)
        {
            if (model.IsValid)
            {
                var moveBetsDay = this._balanceUpService.BalanceUpInfo.MoveBetsDay;
                this._liveReportDataAccess.GetLiveReportSummaryDetail(
                    model,
                    moveBetsDay,
                    productCode,
                    products,
                    out resultBySport,
                    out resultByByCurrency,
                    out resultByBettype);
            }
            else
            {
                resultBySport = new List<LiveReport>();
                resultByByCurrency = new List<LiveReport>();
                resultByBettype = new List<LiveReportByBetType>();
            }
        }

        /// <summary>
        /// Get live report win-loss by match.
        /// </summary>
        /// <param name="model">The metal model.</param>
        /// <returns>The data list.</returns>
        public IList<LiveReportWinLossByMatch> GetLiveReportWinLossByMatch(LiveReportMatchListMetalModel model)
        {
            if (model.IsValid)
            {
                var moveBetsDay = this._balanceUpService.BalanceUpInfo.MoveBetsDay;
                var dataList = _liveReportDataAccess.GetLiveReportWinlossByMatch(model, moveBetsDay);

                foreach (var data in dataList)
                {
                    data.MatchName = model.SportId == (int)SportType.LiveCasino
                                         ? string.Format("{0} {1} {2} ", CacheRef.GetSportName(model.SportId), CacheRef.GetBettype(data.BetType), data.ShowTime)
                                         : string.Format("{0} vs {1}", CacheRef.GetTeamName(data.HomeId), CacheRef.GetTeamName(data.AwayId));
                }

                return dataList.OrderBy(d => d.MatchName).ToList();
            }

            return new List<LiveReportWinLossByMatch>();
        }
    }
}