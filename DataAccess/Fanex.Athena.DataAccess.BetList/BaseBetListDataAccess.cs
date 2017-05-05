// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DataAccess.Core.Utilities;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.BetList.DTO;

    public abstract class BaseBetListDataAccess : BaseDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [CLSCompliant(false)]
        protected internal static SortableList<LrfTicket> ReadBetListTicket(IDataReader dataReader, string baseCurrency, long custId = 0)
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
            var columns = new string[]
            {
                "username", "refno", "matchcode", "status", "betteam", "betstatus", "showtime", "oddstype",
                "nickname", "race", "betfrom", "betcheck", "liveindicator", "isneutral", "sporttype", "bettype",
                "leagueid", "awayid", "homeid", "matchid", "custid", "currencyid", "livehomescore", "liveawayscore",
                "SpecialLevel", "StatusId", "transid", "betid", "winlostdate", "transdate",
                playerCommCol, commCol, licenseeWlCol, winlostCol, beteffectiveCol, stakeCol, actualStakeCol,
                "odds", "modds", "hdp1", "hdp2", "transdesc", "currency", "betdaqid", "extrainfo", originalStakeCol, "originalbettype"
            };

            var reader = new SafeDataReader(dataReader, columns);

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
                    Currency = reader.GetString("Currency"),

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

                    // Long
                    TransId = reader.GetLong("transid")
                };
                ticket.BetId = reader.GetLong("betid");

                // DateTime
                ticket.WinlostDate = reader.GetDateTime("winlostdate", DateTime.Now);
                ticket.TransDate = reader.GetDateTime("transdate", DateTime.Now);

                // Money
                ticket.PlayerComm = reader.GetDecimal(playerCommCol);
                ticket.Comm = reader.GetDecimal(commCol);
                ticket.LicenseeWL = reader.GetDecimal(licenseeWlCol);
                ticket.Winlost = reader.GetDecimal(winlostCol);
                ticket.BetEffective = reader.GetDecimal(beteffectiveCol);
                ticket.Stake = reader.GetDecimal(stakeCol);
                ticket.ActualStake = reader.GetDecimal(actualStakeCol);
                ticket.Odds = reader.GetDecimal("odds");
                ticket.MasterOdds = reader.GetDecimal("modds");
                ticket.Handicap1 = reader.GetDecimal("hdp1");
                ticket.Handicap2 = reader.GetDecimal("hdp2");
                ticket.TransDesc = reader.GetString("transdesc");
                ticket.Currency = reader.GetString("currency");

                ticket.BetdaqId = reader.GetLong("betdaqid", -1);
                ticket.ExtraInfo = reader.GetString("extrainfo");
                ticket.OriginalStake = reader.GetDecimal(originalStakeCol, -1);
                ticket.OriginalBetType = reader.GetInt("originalbettype");

                tickets.Add(ticket);
            }

            return tickets;
        }

        /// <summary>
        /// Numbers the report.
        /// </summary>
        /// <param name="value">The decimal number. value.</param>
        /// <returns>The result value.</returns>
        protected static string NumberReport(object value)
        {
            var number = Convert.ToDecimal(value);
            return number >= 0 ? "<span class='bl-underdog'>" + $"{number:#,##0.00}" + "</span>"
                       : "<span class='bl-favorite'>" + $"{number:#,##0.00}" + "</span>";
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

            if (value.GetType() == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            if (value.GetType() == typeof(int))
            {
                return Convert.ToInt32(value) > 0;
            }

            return false;
        }
    }
}