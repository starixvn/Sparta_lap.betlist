// -----------------------------------------------------------------------
// <copyright file="MatchWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Resources;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class MatchWinLossService : IMatchWinLossService
    {
        private const string KickOfTimeFormat = "{0:MM/dd/yyyy hh:mm:ss tt}";

        private static readonly IEnumerable<int> RacingSportTypes = new List<int>
                    {
                        (int)SportType.VirtualHorseRacing,
                        (int)SportType.VirtualGreyhound,
                        (int)SportType.VirtualMotoRacing,
                        (int)SportType.VirtualSpeedway,
                        (int)SportType.VirtualCycling
                    };

        private static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

        private readonly IMatchWinLossDataAccess _matchWinLossDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public MatchWinLossService(IMatchWinLossDataAccess matchWinLossDataAccess, ICacheRefAdapter cacheRefAdapter)
        {
            this._matchWinLossDataAccess = matchWinLossDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public MatchWinLossService()
        {
            this._matchWinLossDataAccess = new MatchWinLossDataAccess();
            _cacheRefAdapter = new CacheRefAdapter();
        }

        #region IMatchWinLossService Members

        public IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel)
        {
            var dataList = _matchWinLossDataAccess.GetDataList(metaModel);

            foreach (var data in dataList)
            {
                data.LeagueName = GetLeagueName(data);
                data.MatchName = GetMatchName(data);
                data.HomeName = GetHomeName(data);
                data.AwayName = GetAwayName(data);
            }

            return OrderByProduct(dataList);
        }

        public DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel)
        {
            return _matchWinLossDataAccess.GetDetailDataSet(metaModel);
        }

        public IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel)
        {
            return _matchWinLossDataAccess.GetDetailDataList(metaModel);
        }

        public IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel)
        {
            return _matchWinLossDataAccess.GetOutrightParlayList(metaModel);
        }

        #endregion IMatchWinLossService Members

        #region Utility

        private static IList<MatchWinLoss> OrderByProduct(IList<MatchWinLoss> dataList)
        {
            var result = dataList.Where(p => p.ProductName == Product.SB || p.ProductName == Product.BA)
                                 .GroupBy(p => p.MatchId)
                                 .Select(g => new MatchWinLoss
                                 {
                                     LeagueId = g.Select(r => r.LeagueId).FirstOrDefault(),
                                     LeagueName = g.Select(r => r.LeagueName).FirstOrDefault(),
                                     SportType = g.Select(r => r.SportType).FirstOrDefault(),
                                     MatchId = g.Select(r => r.MatchId).FirstOrDefault(),
                                     ShowTime = g.Select(r => r.ShowTime).FirstOrDefault(),
                                     MatchName = g.Select(r => r.MatchName).FirstOrDefault(),
                                     MatchCode = g.Select(r => r.MatchCode).FirstOrDefault(),
                                     HomeName = g.Select(r => r.HomeName).FirstOrDefault(),
                                     AwayName = g.Select(r => r.AwayName).FirstOrDefault(),
                                     ProductName = Product.SB,
                                     RaceNo = g.Select(r => r.RaceNo).FirstOrDefault(),
                                     IsLive = g.Select(r => r.IsLive).FirstOrDefault(),
                                     HomeId = g.Select(r => r.HomeId).FirstOrDefault(),
                                     AwayId = g.Select(r => r.AwayId).FirstOrDefault(),
                                     BetType = g.Select(r => r.BetType).FirstOrDefault(),
                                     IsNeutral = g.Select(r => r.IsNeutral).FirstOrDefault(),
                                     Turnover = g.Sum(r => r.Turnover),
                                     TotalTransaction = g.Sum(r => r.TotalTransaction),
                                     CustomerWL = g.Sum(r => r.CustomerWL),
                                     CustomerComm = g.Sum(r => r.CustomerComm),
                                     LicenseeWL = g.Sum(r => r.LicenseeWL),
                                     BACommission = g.Sum(r => r.BACommission),
                                     RacingRevenue = g.Sum(r => r.RacingRevenue),
                                     AmtDueToBA = g.Sum(r => r.AmtDueToBA),
                                     AmtDueToRacing = g.Sum(r => r.AmtDueToRacing),
                                 })
                                 .OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName).ToList();

            result.AddRange(
                dataList.Where(p => p.ProductName == Product.RB).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.NG).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.VS).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.VS2).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));

            return result;
        }

        private string GetHomeName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                case Product.NG:
                    return string.Empty;

                default:
                    return _cacheRefAdapter.GetTeamName(matchWinLoss.HomeId) + (matchWinLoss.IsNeutral ? " " + ViewResources.IsNeutral : string.Empty);
            }
        }

        private string GetAwayName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                case Product.NG:
                    return string.Empty;

                default:
                    return _cacheRefAdapter.GetTeamName(matchWinLoss.AwayId);
            }
        }

        private string GetLeagueName(MatchWinLoss matchWinLoss)
        {
            return matchWinLoss.ProductName == Product.RB ?
                ViewResources.ProductRB + " - " + _cacheRefAdapter.GetLeagueName(matchWinLoss.LeagueId) :
                _cacheRefAdapter.GetLeagueName(matchWinLoss.LeagueId);
        }

        private string GetMatchName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                    return ViewResources.Race + " " + matchWinLoss.RaceNo;

                case Product.NG:
                    return ViewResources.NumberGameNo + " " + matchWinLoss.MatchCode;

                default:

                    if (RacingSportTypes.Contains(matchWinLoss.SportType))
                    {
                        return string.Format(EnglishCulture, KickOfTimeFormat, matchWinLoss.KickOffTime);
                    }

                    var homeName = _cacheRefAdapter.GetTeamName(matchWinLoss.HomeId) +
                                   (matchWinLoss.IsNeutral ? " " + ViewResources.IsNeutral : string.Empty);
                    var awayName = _cacheRefAdapter.GetTeamName(matchWinLoss.AwayId);
                    return homeName + "-vs-" + awayName;
            }
        }

        #endregion Utility
    }
}