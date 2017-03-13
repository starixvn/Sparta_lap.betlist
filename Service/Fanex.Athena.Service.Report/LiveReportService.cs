// -----------------------------------------------------------------------
// <copyright file="LiveReportService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

    public class LiveReportService : ILiveReportService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly ILiveReportDataAccess _liveReportDataAccess;

        public LiveReportService()
        {
            this._liveReportDataAccess = new LiveReportDataAccess();
            this._balanceUpService = new BalanceUpService();
        }

        public IList<LiveReport> GetLiveReportSummary(LiveReportMetaModel model)
        {
            if (model.IsValid)
            {
                var balance = this._balanceUpService.BalanceUpInfo;
                return this._liveReportDataAccess.GetLiveReportSummary(model, balance.MoveBetsDay);
            }

            return new List<LiveReport>();
        }

        public void GetLiveReportSumaryDetail(
            LiveReportMetaModel model,
            string productCode,
            IList<Product> products,
            out IList<LiveReport> resultBySport,
            out IList<LiveReport> resultByByCurrency,
            out IList<LiveReportByBetType> resultByBettype)
        {
            if (model.IsValid)
            {
                var moveBetsDay = this._balanceUpService.BalanceUpInfo.MoveBetsDay;

                var taskForSport = Task<IList<LiveReport>>.Factory.StartNew(() =>
                {
                    return _liveReportDataAccess.GetDetailBySport(model, moveBetsDay, productCode);
                });

                var taskForCurrencyAndBettype = Task<Tuple<IList<LiveReport>, IList<LiveReportByBetType>>>.Factory.StartNew(() =>
                {
                    return _liveReportDataAccess.GetDetailByCurrencyAndBettype(
                        model,
                        moveBetsDay,
                        productCode,
                        products);
                });

                resultBySport = taskForSport.Result;
                resultByByCurrency = taskForCurrencyAndBettype.Result.Item1;
                resultByBettype = taskForCurrencyAndBettype.Result.Item2;
            }
            else
            {
                resultBySport = new List<LiveReport>();
                resultByByCurrency = new List<LiveReport>();
                resultByBettype = new List<LiveReportByBetType>();
            }
        }

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