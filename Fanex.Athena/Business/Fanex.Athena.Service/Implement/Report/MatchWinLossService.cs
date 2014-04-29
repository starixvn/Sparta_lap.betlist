// -----------------------------------------------------------------------
// <copyright file="MatchWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class MatchWinLossService : IMatchWinLossService
    {
        private const string KICKOFFTIMEFORMATER = "{0:MM/dd/yyyy hh:mm:ss tt}";

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

        /// <summary>
        /// Get data for match win-loss.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The match win-loss data list.</returns>
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

        /// <summary>
        /// Get out right data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The data set.</returns>
        public DataSet GetOutrightParlayDataSet(MatchWinLossMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? this._matchWinLossDataAccess.GetOutrightParlayDataSet(metaModel)
                       : new DataSet();
        }

        #endregion IMatchWinLossService Members

        #region Utility

        /// <summary>
        /// Get home team name.
        /// </summary>
        /// <param name="matchWinLoss">The match win-loss.</param>
        /// <returns>The home team name.</returns>
        private static string GetHomeName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.Rb:
                    return string.Empty;
                case Product.Ng:
                    return string.Empty;
                default:
                    var homeName = CacheRef.GetTeamName(matchWinLoss.HomeId) +
                                   (matchWinLoss.IsNeutral ? " " + ViewResources.IsNeutral : string.Empty);
                    return homeName;
            }
        }

        /// <summary>
        /// Get away team name.
        /// </summary>
        /// <param name="matchWinLoss">The match win-loss.</param>
        /// <returns>The away team name.</returns>
        private static string GetAwayName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.Rb:
                    return string.Empty;
                case Product.Ng:
                    return string.Empty;
                default:
                    var awayName = CacheRef.GetTeamName(matchWinLoss.AwayId);

                    return awayName;
            }
        }

        /// <summary>
        /// Order data list.
        /// </summary>
        /// <param name="dataList">The original data list.</param>
        /// <returns>The ordered data list.</returns>
        private IList<MatchWinLoss> OrderByProduct(IList<MatchWinLoss> dataList)
        {
            var result = dataList.Where(p => p.ProductName == Product.Sb || p.ProductName == Product.Ba)
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
                                         ProductName = Product.Sb,
                                         RaceNo = g.Select(r => r.RaceNo).FirstOrDefault(),
                                         IsLive = g.Select(r => r.IsLive).FirstOrDefault(),
                                         HomeId = g.Select(r => r.HomeId).FirstOrDefault(),
                                         AwayId = g.Select(r => r.AwayId).FirstOrDefault(),
                                         BetType = g.Select(r => r.BetType).FirstOrDefault(),
                                         IsNeutral = g.Select(r => r.IsNeutral).FirstOrDefault(),
                                         Turnover = g.Sum(r => r.Turnover),
                                         CustomerWl = g.Sum(r => r.CustomerWl),
                                         CustomerComm = g.Sum(r => r.CustomerComm),
                                         LicenseeWl = g.Sum(r => r.LicenseeWl),
                                         BaCommission = g.Sum(r => r.BaCommission),
                                         RacingRevenue = g.Sum(r => r.RacingRevenue),
                                         AmtDueToBa = g.Sum(r => r.AmtDueToBa),
                                         AmtDueToRacing = g.Sum(r => r.AmtDueToRacing),
                                     })
                                 .OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName).ToList();

            result.AddRange(
                dataList.Where(p => p.ProductName == Product.Rb).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.Ng).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.Lc).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));
            result.AddRange(
                dataList.Where(p => p.ProductName == Product.Vs).OrderBy(m => m.LeagueName).ThenBy(m => m.MatchName));

            return result;
        }

        /// <summary>
        /// Get league name.
        /// </summary>
        /// <param name="matchWinLoss">The match win-loss DTO.</param>
        /// <returns>The league name.</returns>
        private string GetLeagueName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.Rb:
                    return ViewResources.Racing + " - " + CacheRef.GetLeagueName(matchWinLoss.LeagueId);
                default:
                    return CacheRef.GetLeagueName(matchWinLoss.LeagueId);
            }
        }

        /// <summary>
        /// Get match name.
        /// </summary>
        /// <param name="matchWinLoss">The match win-loss.</param>
        /// <returns>The match name.</returns>
        private string GetMatchName(MatchWinLoss matchWinLoss)
        {
            switch (matchWinLoss.ProductName)
            {
                case Product.Rb:
                    return ViewResources.Race + " " + matchWinLoss.RaceNo;
                case Product.Ng:
                    return ViewResources.NumberGameNo + " " + matchWinLoss.MatchCode;
                case Product.Lc:
                    return string.Format(
                        "{0} {1} {2} ",
                        CacheRef.GetSportName(matchWinLoss.SportType),
                        CacheRef.GetBettype(matchWinLoss.BetType),
                        matchWinLoss.ShowTime);

                default:
                    var racingSportTypes = new List<int>
                    {
                        (int)SportType.VirtualHorseRacing,
                        (int)SportType.VirtualGreyhound,
                        (int)SportType.VirtualMotoRacing,
                        (int)SportType.VirtualSpeedway,
                        (int)SportType.VirtualCycling
                    };

                    if (racingSportTypes.Contains(matchWinLoss.SportType))
                    {                        
                        return string.Format(_culturelEnglish, KICKOFFTIMEFORMATER, matchWinLoss.KickofTime);
                    }

                    var homeName = CacheRef.GetTeamName(matchWinLoss.HomeId) +
                                   (matchWinLoss.IsNeutral ? " " + ViewResources.IsNeutral : string.Empty);
                    var awayName = CacheRef.GetTeamName(matchWinLoss.AwayId);
                    var matchName = homeName + "-vs-" + awayName;
                    return matchName;
            }
        }        

        #endregion Utility
    }
}