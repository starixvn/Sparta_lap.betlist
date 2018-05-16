// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Core.Utilities;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.BetList.DTO;
    using Model.Core.DataDomain;
    using Session.Interfaces;

    public class BaseBetListDataAccess : BaseDataAccess
    {
        public BaseBetListDataAccess() : this(null, null)
        {
        }

        public BaseBetListDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [CLSCompliant(false)]
        protected internal SortableList<LrfTicket> ReadBetListTicket(IDataReader dataReader, string baseCurrency, long custId = 0)
        {
            var columnSuffix = SystemConstants.BaseCurrencyList.Contains(baseCurrency?.ToUpperInvariant()) ? baseCurrency : string.Empty;

            var playerCommCol = "playercomm" + columnSuffix;
            var commCol = "comm" + columnSuffix;
            var licenseeWlCol = "licenseewl" + columnSuffix;
            var winlostCol = "winlost" + columnSuffix;
            var beteffectiveCol = "beteffective" + columnSuffix;
            var stakeCol = "stake" + columnSuffix;
            var actualStakeCol = "actual_stake" + columnSuffix;
            var originalStakeCol = "originalstake" + columnSuffix;

            var tickets = new SortableList<LrfTicket>();
            var columns = new[]
            {
                "username", "refno", "matchcode", "status", "betteam", "betstatus", "showtime", "oddstype",
                "nickname", "race", "betfrom", "betcheck", "liveindicator", "isneutral", "sporttype", "bettype",
                "leagueid", "awayid", "homeid", "matchid", "custid", "currencyid", "livehomescore", "liveawayscore",
                "SpecialLevel", "StatusId", "transid", "betid", "winlostdate", "transdate",
                playerCommCol, commCol, licenseeWlCol, winlostCol, beteffectiveCol, stakeCol, actualStakeCol,
                "odds", "modds", "hdp1", "hdp2", "transdesc", "currency", "betdaqid", "extrainfo", originalStakeCol, "originalbettype",
                "refcode", "wallettype", "oddsspread"
            };

            var reader = this.GetSafeDataReader(dataReader, columns);

            while (reader.Read())
            {
                var currencyId = reader.GetInt("currencyid");
                var ticket = new LrfTicket
                {
                    // string
                    UserName = reader.GetString("username"),
                    RefNo = reader.GetString("refno"),
                    MatchCode = reader.GetString("matchcode"),
                    Status = reader.GetString("status"),
                    BetTeam = reader.GetString("betteam"),
                    EventStatus = reader.GetString("betstatus"),
                    ShowTime = reader.GetString("showtime"),
                    OddsType = reader.GetString("oddstype", "0"),
                    ExternalId = reader.GetString("nickname"),
                    Race = reader.GetString("race"),
                    BetFrom = reader.GetString("betfrom"),
                    BetCheck = reader.GetString("betcheck"),

                    // Boolean
                    IsLive = reader.GetBool("liveindicator"),
                    IsNeutral = reader.GetBool("isneutral"),

                    // Int
                    SportTypeId = reader.GetInt("sporttype"),
                    BetTypeId = reader.GetInt("bettype"),
                    LeagueId = reader.GetInt("leagueid"),
                    HomeId = reader.GetInt("homeid"),
                    AwayId = reader.GetInt("awayid"),
                    MatchId = reader.GetInt("matchid"),
                    CustId = reader.GetLong("custid", custId),
                    CurrencyId = currencyId,
                    LiveHomeScore = reader.GetInt("livehomescore"),
                    LiveAwayScore = reader.GetInt("liveawayscore"),
                    SpecialLevel = reader.GetInt("SpecialLevel"),
                    StatusId = reader.GetInt("StatusId"),
                    WalletType = (byte)reader.GetInt("wallettype"),

                    // Long
                    TransId = reader.GetLong("transid"),
                    RefCode = reader.GetLong("refcode"),
                    BetId = reader.GetNullableLong("betid"),

                    // DateTime
                    WinlostDate = reader.GetDateTime("winlostdate", DateTime.Now),
                    TransDate = reader.GetDateTime("transdate", DateTime.Now),

                    // Money
                    PlayerComm = reader.GetDecimal(playerCommCol),
                    Comm = reader.GetDecimal(commCol),
                    LicenseeWL = reader.GetDecimal(licenseeWlCol),
                    Winlost = reader.GetDecimal(winlostCol),
                    BetEffective = reader.GetDecimal(beteffectiveCol),
                    Stake = reader.GetDecimal(stakeCol),
                    ActualStake = reader.GetDecimal(actualStakeCol),
                    Odds = reader.GetDecimal("odds"),
                    MasterOdds = reader.GetDecimal("modds"),
                    Handicap1 = reader.GetDecimal("hdp1"),
                    Handicap2 = reader.GetDecimal("hdp2"),
                    TransDesc = reader.GetString("transdesc"),
                    Currency = GetCurrencyName(currencyId),

                    BetdaqId = reader.GetLong("betdaqid", -1),
                    ExtraInfo = reader.GetString("extrainfo"),
                    OriginalStake = reader.GetDecimal(originalStakeCol, -1),
                    OriginalBetType = reader.GetInt("originalbettype"),
                    OddsSpread = reader.GetDecimal("oddsspread")
                };

                tickets.Add(ticket);
            }

            return tickets;
        }

        protected internal virtual ISafeDataReader GetSafeDataReader(IDataReader reader, IEnumerable<string> columns)
        {
            return new SafeDataReader(reader, columns);
        }
    }
}