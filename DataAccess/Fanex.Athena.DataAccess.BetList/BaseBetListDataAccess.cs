// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseBetListDataAccess : BaseDataAccess
    {
        /// <summary>
        /// Do not use reflection because of bad performance.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <param name="custId">The customer ID.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [CLSCompliant(false)]
        protected static SortableList<LrfTicket> ReadLrfTicket(IDataReader dataReader, string baseCurrency, long custId = 0)
        {
            var playerCommCol = "playercomm" + baseCurrency;
            var commCol = "comm" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var winlostCol = "winlost" + baseCurrency;
            var beteffectiveCol = "beteffective" + baseCurrency;
            var stakeCol = "stake" + baseCurrency;
            var actualStakeCol = "actual_stake" + baseCurrency;

            var tickets = new SortableList<LrfTicket>();
            var columns = new string[]
            {
                "username", "refno", "matchcode", "status", "betteam", "betstatus", "showtime", "oddstype",
                "nickname", "race", "betfrom", "betcheck", "liveindicator", "isneutral", "sporttype", "bettype",
                "leagueid", "awayid", "homeid", "matchid", "custid", "currencyid", "livehomescore", "liveawayscore",
                "SpecialLevel", "StatusId", "transid", "betid", "winlostdate", "transdate",
                playerCommCol, commCol, licenseeWlCol, winlostCol, beteffectiveCol, stakeCol, actualStakeCol,
                "odds", "modds", "hdp1", "hdp2", "transdesc"
            };

            var reader = new SafeDataReader(dataReader, columns);

            while (reader.Read())
            {
                var ticket = new LrfTicket();

                // string
                ticket.UserName = reader.GetString("username");
                ticket.RefNo = reader.GetString("refno");
                ticket.MatchCode = reader.GetString("matchcode");
                ticket.Status = reader.GetString("status");
                ticket.BetTeam = reader.GetString("betteam");
                ticket.EventStatus = reader.GetString("betstatus");
                ticket.ShowTime = reader.GetString("showtime");
                ticket.OddsType = reader.GetString("oddstype", "0");
                ticket.ExternalId = reader.GetString("nickname");
                ticket.Race = reader.GetString("race");
                ticket.BetFrom = reader.GetString("betfrom");
                ticket.BetCheck = reader.GetString("betcheck");

                // Boolean
                ticket.IsLive = reader.GetValue("liveindicator", x => ReadBoolean(x), false);
                ticket.IsNeutral = reader.GetValue("isneutral", x => ReadBoolean(x), false);

                // Int
                ticket.SportTypeId = reader.GetInt("sporttype");
                ticket.BetTypeId = reader.GetInt("bettype");
                ticket.LeagueId = reader.GetInt("leagueid");
                ticket.HomeId = reader.GetInt("homeid");
                ticket.AwayId = reader.GetInt("awayid");
                ticket.MatchId = reader.GetInt("matchid");
                ticket.CustId = reader.GetLong("custid", custId);
                ticket.CurrencyId = reader.GetInt("currencyid");
                ticket.LiveHomeScore = reader.GetInt("livehomescore");
                ticket.LiveAwayScore = reader.GetInt("liveawayscore");
                ticket.SpecialLevel = reader.GetInt("SpecialLevel");
                ticket.StatusId = reader.GetInt("StatusId");

                // Long
                ticket.TransId = reader.GetLong("transid");
                var betid = reader.GetValue("betid");
                ticket.BetId = betid == DBNull.Value ? null : (long?)betid;

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
            decimal number = Convert.ToDecimal(value);
            return (number >= 0)
                       ? "<span class='bl-underdog'>" + string.Format("{0:#,##0.00}", number) + "</span>"
                       : "<span class='bl-favorite'>" + string.Format("{0:#,##0.00}", number) + "</span>";
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