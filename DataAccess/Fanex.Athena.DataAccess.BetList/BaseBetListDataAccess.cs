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

    public class BaseBetListDataAccess : BaseDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [CLSCompliant(false)]
        protected internal SortableList<LrfTicket> ReadBetListTicket(IDataReader dataReader, string baseCurrency, long custId = 0)
        {
            var playerCommCol = "playercomm" + baseCurrency;
            var commCol = "comm" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var winlostCol = "winlost" + baseCurrency;
            var beteffectiveCol = "beteffective" + baseCurrency;
            var stakeCol = "stake" + baseCurrency;
            var actualStakeCol = "actual_stake" + baseCurrency;
            var originalStakeCol = "originalstake" + baseCurrency;

            var tickets = new SortableList<LrfTicket>();
            var columns = new[]
            {
                "username", "refno", "matchcode", "status", "betteam", "betstatus", "showtime", "oddstype",
                "nickname", "race", "betfrom", "betcheck", "liveindicator", "isneutral", "sporttype", "bettype",
                "leagueid", "awayid", "homeid", "matchid", "custid", "currencyid", "livehomescore", "liveawayscore",
                "SpecialLevel", "StatusId", "transid", "betid", "winlostdate", "transdate",
                playerCommCol, commCol, licenseeWlCol, winlostCol, beteffectiveCol, stakeCol, actualStakeCol,
                "odds", "modds", "hdp1", "hdp2", "transdesc", "currency", "betdaqid", "extrainfo", originalStakeCol, "originalbettype",
                "refcode", "wallettype"
            };

            var reader = this.GetSafeDataReader(dataReader, columns);

            while (reader.Read())
            {
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
                    IsLive = reader.GetValue("liveindicator", x => ReadBoolean(x), false),
                    IsNeutral = reader.GetValue("isneutral", x => ReadBoolean(x), false),

                    // Int
                    SportTypeId = reader.GetInt("sporttype"),
                    BetTypeId = reader.GetInt("bettype"),
                    LeagueId = reader.GetInt("leagueid"),
                    HomeId = reader.GetInt("homeid"),
                    AwayId = reader.GetInt("awayid"),
                    MatchId = reader.GetInt("matchid"),
                    CustId = reader.GetLong("custid", custId),
                    CurrencyId = reader.GetInt("currencyid"),
                    LiveHomeScore = reader.GetInt("livehomescore"),
                    LiveAwayScore = reader.GetInt("liveawayscore"),
                    SpecialLevel = reader.GetInt("SpecialLevel"),
                    StatusId = reader.GetInt("StatusId"),
                    WalletType = (byte)reader.GetInt("wallettype"),

                    // Long
                    TransId = reader.GetLong("transid"),
                    RefCode = reader.GetInt("refcode"),
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
                    Currency = reader.GetString("currency"),

                    BetdaqId = reader.GetLong("betdaqid", -1),
                    ExtraInfo = reader.GetString("extrainfo"),
                    OriginalStake = reader.GetDecimal(originalStakeCol, -1),
                    OriginalBetType = reader.GetInt("originalbettype")
                };

                tickets.Add(ticket);
            }

            return tickets;
        }

        /// <summary>
        /// Read boolean value.
        /// </summary>
        /// <param name="value">Convert value.</param>
        /// <returns>The result value.</returns>
        private static bool ReadBoolean(object value)
        {
            if (value == null || value == Convert.DBNull)
            {
                return false;
            }

            if (value is bool)
            {
                return Convert.ToBoolean(value);
            }

            if (value is int)
            {
                return Convert.ToInt32(value) > 0;
            }

            return false;
        }

        protected internal virtual ISafeDataReader GetSafeDataReader(IDataReader reader, IEnumerable<string> columns)
        {
            return new SafeDataReader(reader, columns);
        }
    }
}