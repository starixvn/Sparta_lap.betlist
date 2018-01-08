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
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.ResultModel;

    public class LiveReportService : ILiveReportService
    {
        private readonly ILiveReportDataAccess _liveReportDataAccess;

        public LiveReportService()
        {
            this._liveReportDataAccess = new LiveReportDataAccess();
        }

        public LiveReportService(ILiveReportDataAccess dataAccess)
        {
            this._liveReportDataAccess = dataAccess;
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

            var taskForCurrencyAndBettype = Task<Tuple<IList<LiveReport>, IList<LiveReportByBetType>>>.Factory.StartNew(() =>
            {
                return _liveReportDataAccess.GetDetailByCurrencyAndBettype(model, products);
            });

            return new LiveReportDetailResultModel
            {
                DataBySport = taskForSport.Result,
                DataByCurrency = taskForCurrencyAndBettype.Result.Item1,
                DataByBetType = taskForCurrencyAndBettype.Result.Item2
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
                                         ? string.Format("{0} {1} {2} ", CacheRef.GetSportName(model.SportId), CacheRef.GetBettype(data.BetType), data.ShowTime)
                                         : string.Format("{0} vs {1}", CacheRef.GetTeamName(data.HomeId), CacheRef.GetTeamName(data.AwayId));
                }

                return dataList.OrderBy(d => d.MatchName).ToList();
            }

            return new List<LiveReportWinLossByMatch>();
        }
    }
}