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
    using System.Threading;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.ResultModel;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class LiveReportService : ILiveReportService
    {
        private readonly ILiveReportDataAccess _liveReportDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public LiveReportService()
        {
            this._liveReportDataAccess = new LiveReportDataAccess();
            _cacheRefAdapter = new CacheRefAdapter();
        }

        public LiveReportService(ILiveReportDataAccess dataAccess, ICacheRefAdapter cacheRefAdapter)
        {
            this._liveReportDataAccess = dataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public IList<LiveReport> GetLiveReportSummary(LiveReportMetaModel model)
        {
            return model.IsValid ?
                _liveReportDataAccess.GetLiveReportSummary(model) :
                new List<LiveReport>();
        }

        public LiveReportDetailResultModel GetLiveReportSumaryDetail(
            LiveReportMetaModel model,
            IEnumerable<Product> products)
        {
            if (!model.IsValid)
            {
                return new LiveReportDetailResultModel
                {
                    DataBySport = new List<LiveReport>(),
                    DataByCurrency = new List<LiveReport>(),
                    DataByBetType = new List<LiveReportByBetType>()
                };
            }

            var taskForSport = Task<IList<LiveReport>>.Factory.StartNew(() =>
            {
                return _liveReportDataAccess.GetDetailBySport(model);
            });

            var tupleCurrencyAndBettype = _liveReportDataAccess.GetDetailByCurrencyAndBettype(model, products);
            return new LiveReportDetailResultModel
            {
                DataBySport = taskForSport.Result,
                DataByCurrency = tupleCurrencyAndBettype.Item1,
                DataByBetType = tupleCurrencyAndBettype.Item2
            };
        }

        public IList<LiveReportWinLossByMatch> GetLiveReportWinLossByMatch(LiveReportMatchListMetalModel model)
        {
            if (model.IsValid)
            {
                var dataList = _liveReportDataAccess.GetLiveReportWinlossByMatch(model);

                foreach (var data in dataList)
                {
                    data.MatchName = model.SportId == (int)SportType.LiveCasino
                                         ? string.Format("{0} {1} {2} ", _cacheRefAdapter.GetSportName(model.SportId), _cacheRefAdapter.GetBettype(data.BetType), data.ShowTime)
                                         : string.Format("{0} vs {1}", _cacheRefAdapter.GetTeamName(data.HomeId), _cacheRefAdapter.GetTeamName(data.AwayId));
                }

                return dataList.OrderBy(d => d.MatchName).ToList();
            }

            return new List<LiveReportWinLossByMatch>();
        }
    }
}