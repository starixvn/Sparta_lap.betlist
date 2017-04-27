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
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;
    using Resources;

    public class MatchWinLossService : IMatchWinLossService
    {
        private const string KICKOFFTIME_FORMAT = "{0:MM/dd/yyyy hh:mm:ss tt}";

        private static readonly IList<int> RacingSportTypes = new List<int>
                    {
                        (int)SportType.VirtualHorseRacing,
                        (int)SportType.VirtualGreyhound,
                        (int)SportType.VirtualMotoRacing,
                        (int)SportType.VirtualSpeedway,
                        (int)SportType.VirtualCycling
                    };

        private readonly CultureInfo _culturelEnglish;

        #region Field and constants

        /// <summary>
        ///     The _balance up service.
        /// </summary>
        private readonly IBalanceUpService _balanceUpService;

        /// <summary>
        ///     The match win loss data access.
        /// </summary>
        private readonly MatchWinLossDataAccess _matchWinLossDataAccess;

        #endregion Field and constants

        #region Constructor

        public MatchWinLossService()
        {
            this._balanceUpService = new BalanceUpService();
            this._matchWinLossDataAccess = new MatchWinLossDataAccess(this._balanceUpService.BalanceUpInfo);
            this._culturelEnglish = new CultureInfo("en-US");
        }

        #endregion Constructor

        #region IMatchWinLossService Members

        public IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new List<MatchWinLoss>();
            }

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
            return metaModel.IsValid
                       ? this._matchWinLossDataAccess.GetDetailDataSet(metaModel)
                       : new DataSet();
        }

        public IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? this._matchWinLossDataAccess.GetDetailDataList(metaModel)
                       : new List<MatchWinLoss>();
        }

        public IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? this._matchWinLossDataAccess.GetOutrightParlayList(metaModel)
                       : new List<MatchWinLoss>();
        }

        #endregion IMatchWinLossService Members

        #region Utility

        private static string GetHomeName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                case Product.NG:
                    return string.Empty;

                default:
                    return CacheRef.GetTeamName(matchWinLoss.HomeId) + (matchWinLoss.IsNeutral ? " " + ViewResources.IsNeutral : string.Empty);
            }
        }

        private static string GetAwayName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                case Product.NG:
                    return string.Empty;

                default:
                    return CacheRef.GetTeamName(matchWinLoss.AwayId);
            }
        }

        /// <summary>
        /// Order data list.
        /// </summary>
        /// <param name="dataList">The original data list.</param>
        /// <returns>The ordered data list.</returns>
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
                dataList.Where(p => p.ProductName == Product.LC).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.VS).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));

            return result;
        }

        private static string GetLeagueName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                    return ViewResources.ProductRB + " - " + CacheRef.GetLeagueName(matchWinLoss.LeagueId);

                default:
                    return CacheRef.GetLeagueName(matchWinLoss.LeagueId);
            }
        }

        private string GetMatchName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.RB:
                    return ViewResources.Race + " " + matchWinLoss.RaceNo;

                case Product.NG:
                    return ViewResources.NumberGameNo + " " + matchWinLoss.MatchCode;

                case Product.LC:
                    return string.Format(
                        "{0} {1} {2} ",
                        CacheRef.GetSportName(matchWinLoss.SportType),
                        CacheRef.GetBettype(matchWinLoss.BetType),
                        matchWinLoss.ShowTime);

                default:

                    if (RacingSportTypes.Contains(matchWinLoss.SportType))
                    {
                        return string.Format(_culturelEnglish, KICKOFFTIME_FORMAT, matchWinLoss.KickofTime);
                    }

                    var homeName = CacheRef.GetTeamName(matchWinLoss.HomeId) +
                                   (matchWinLoss.IsNeutral ? " " + ViewResources.IsNeutral : string.Empty);
                    var awayName = CacheRef.GetTeamName(matchWinLoss.AwayId);
                    return homeName + "-vs-" + awayName;
            }
        }

        #endregion Utility
    }
}