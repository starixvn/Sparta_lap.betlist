// -----------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.BetList
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.BetList;
    using Fanex.Athena.DataAccess.Interface.BetList;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Interface.BetList;
    using Fanex.BetList.Core.Entities;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetListService : IBetListService
    {
        #region Fields

        private const string MatchListSpKey = "Lrf_Monitor_MatchSel";
        private const string OutrightMatchListSpKey = "Lrf_Monitor_OutrightSel";
        private static List<BetListMatch> matchList = new List<BetListMatch>();
        private static List<BetListMatch> outrightMatchList = new List<BetListMatch>();
        private static IList<int> convertingDecimalOddsBetType = new List<int> { 1, 2, 3, 7, 8, 12, 20 };

        private readonly IBalanceUpService _balanceUpService;
        private readonly IBetListDataAccess _betlistDataAccess;
        private readonly IAuthorizationService _authorizationService;

        private bool _reloadedMatch = false;
        private bool _reloadedOutrightMatch = false;

        #endregion

        #region Constructor

        public BetListService()
        {
            _balanceUpService = new BalanceUpService();
            _betlistDataAccess = new BetListDataAccess();
            _authorizationService = new AuthorizationService();
        }

        #endregion

        #region IBetListService Members

        /// <summary>
        /// Get Mix Parlay Detail data.
        /// </summary>
        /// <param name="transId">The transaction identity.</param>
        /// <returns>The ticket result.</returns>
        public TicketResult GetMixParlayDetailTicket(long transId)
        {
            return _authorizationService.HasPermissionOnData(0, transId)
                        ? _betlistDataAccess.GetMixParlayDetailTicket(transId)
                        : new TicketResult();
        }

        /// <summary>
        /// Get System Parlay Detail data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The system parlay.</returns>
        [System.CLSCompliant(false)]
        public SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetSystemParlayDetail(metaModel)
                        : new SystemParlayData
                        {
                            TicketList = new List<ISystemParlayTicket>(),
                            SerialList = new List<ISystemParlaySerial>()
                        };
        }

        /// <summary>
        /// Get Bet List Data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="uspKey">The store procedure key.</param>
        /// <returns>The ticket result.</returns>
        public TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey)
        {
            UpdateCommonInfo(metaModel);

            return _authorizationService.HasPermissionOnData(metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetBetListData(metaModel, uspKey)
                        : new TicketResult();
        }

        /// <summary>
        /// Get Daily Statement Data of Customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="isAllowCustComm">The allowed customer commission.</param>
        /// <returns>The daily-statement list.</returns>
        public IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool isAllowCustComm)
        {
            return _authorizationService.HasPermissionOnData(metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetDailyStatementData(metaModel, isAllowCustComm)
                         : new List<DailyStatement>();
        }

        /// <summary>
        /// Get Cancelled Bets of Customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The canceled bet list.</returns>
        public IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetCancelBetCustList(metaModel)
                         : new List<CustomerCancelBetItem>();
        }

        #endregion

        #region Lastbetmonitor

        /// <summary>
        /// Get last bets monitoring data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The ticket result.</returns>
        public TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel)
        {
            var ticketResult = this._betlistDataAccess.GetLastBetsMonitoringData(metaModel);

            UpdateMatchInfo(metaModel, ticketResult);

            return ticketResult;
        }

        #endregion

        #region Update Odds

        private static bool IsConvertToOdd(int betTypeId)
        {
            return convertingDecimalOddsBetType.Contains(betTypeId);
        }

        /// <summary>
        /// Convert odds of ticket  to Decimal odd.
        /// In case:
        /// Odd-type equals one or two ,converting HK to DEC.
        /// Odd-type equals three , converting MY to DEC.
        /// Odd-type equals four , converting INDO to DEC.
        /// Odd-type equals five , converting American to DEC.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        private static void ConvertToDecimalOdds(LrfTicket ticket)
        {
            switch (ticket.OddsType)
            {
                case "1":
                case "2":
                    ticket.Odds++;
                    break;

                case "3":
                case "4":
                    ticket.Odds = ticket.Odds < 0 ? 1 - (1 / ticket.Odds) : ticket.Odds + 1;
                    break;

                case "5":
                    {
                        var odds = ticket.Odds * 100;
                        if (odds < 0)
                        {
                            ticket.Odds = 1 - (1 / ticket.Odds);
                        }
                        else if (odds % 2 == 0)
                        {
                            ticket.Odds = 2;
                        }
                        else if (odds > 0)
                        {
                            ticket.Odds++;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Converting odd type.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        private static void ConvertToMyOdds(LrfTicket ticket)
        {
            switch (ticket.OddsType)
            {
                case "1":
                case "2":
                    if (ticket.Odds > 1)
                    {
                        ticket.Odds = -1 / ticket.Odds;
                    }

                    break;

                case "3":
                case "5": // AMERICAN To DEC
                    if (ticket.Odds < -1 || ticket.Odds > 1)
                    {
                        ticket.Odds = -1 / ticket.Odds;
                    }

                    break;
            }
        }

        #endregion

        #region Reload Match Info Cache

        /// <summary>
        /// Get match data by match id.
        /// </summary>
        /// <param name="matchId">The match identity.</param>
        /// <param name="betType">The bet type.</param>
        /// <returns>The information of match. </returns>
        private BetListMatch GetMatchInfo(int matchId, int betType)
        {
            var outright = betType == (int)BetType.Outright
                        || betType == (int)BetType.Win
                        || betType == (int)BetType.Place
                        || betType == (int)BetType.WinPlace;

            if (outright)
            {
                var match = outrightMatchList.Where(x => x.MatchId == matchId).FirstOrDefault();

                if (match == null)
                {
                    ReloadOutrightMatchList();
                    if (outrightMatchList != null)
                    {
                        match = outrightMatchList.Where(x => x.MatchId == matchId).FirstOrDefault();
                    }
                }

                return match;
            }
            else
            {
                var match = matchList.Where(x => x.MatchId == matchId).FirstOrDefault();

                if (match == null)
                {
                    ReloadMatchList();
                    match = matchList.Where(x => x.MatchId == matchId).FirstOrDefault();
                }

                return match;
            }
        }

        /// <summary>
        /// Reload match list once if the match is not found.
        /// </summary>
        private void ReloadMatchList()
        {
            if (!_reloadedMatch)
            {
                matchList.Clear();
                matchList = this._betlistDataAccess.GetMatchList(MatchListSpKey);
                _reloadedMatch = true;
            }
        }

        /// <summary>
        /// Reload outright match list once if the match is not found.
        /// </summary>
        private void ReloadOutrightMatchList()
        {
            if (!_reloadedOutrightMatch)
            {
                outrightMatchList.Clear();
                outrightMatchList = this._betlistDataAccess.GetMatchList(OutrightMatchListSpKey);
                _reloadedOutrightMatch = true;
            }
        }

        #endregion

        #region Update Match

        private void UpdateCommonInfo(BetListMetaModel metaModel)
        {
            var balanceUpInfo = _balanceUpService.BalanceUpInfo;
            metaModel.MoveBetsDay = balanceUpInfo.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUpInfo.BalanceUpDay;
        }

        /// <summary>
        /// Update match data for last bet monitoring.
        /// Update time zone.
        /// Update odds.
        /// </summary>
        /// <param name="metaModel">The metal mode.</param>
        /// <param name="ticketResult">The ticket result.</param>
        private void UpdateMatchInfo(LastBetsMonitoringMetaModel metaModel, TicketResult ticketResult)
        {
            var mainTicketList = ticketResult.MainTicketList;
            var timeZoneDelta = metaModel.TimeZone - (-4);
            foreach (LrfTicket ticket in mainTicketList)
            {
                var match = GetMatchInfo(ticket.MatchId, ticket.BetTypeId);
                if (match != null)
                {
                    ticket.SportTypeId = match.SportType;
                    ticket.LeagueId = match.LeagueId;
                    ticket.MatchCode = match.MatchCode;
                    ticket.HomeId = match.HomeId;
                    ticket.AwayId = match.AwayId;
                    ticket.Race = match.Race;
                }

                // Update time zone
                if (timeZoneDelta > 0)
                {
                    ticket.TransDate = ticket.TransDate.ToUniversalTime().AddHours(timeZoneDelta).ToLocalTime();
                }

                //// Update odds
                if (metaModel.OddsType.ToLower() == "dec" && IsConvertToOdd(ticket.BetTypeId))
                {
                    ConvertToDecimalOdds(ticket);
                }

                if (metaModel.OddsType.ToLower() == "my" && IsConvertToOdd(ticket.BetTypeId))
                {
                    ConvertToMyOdds(ticket);
                }
            }
        }

        #endregion
    }
}