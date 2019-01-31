// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Athena.Model.Core.DataDomain;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.Extension.Utility;
    using Util;

    public class BaseBetListDataAccess : BaseDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        protected internal static LrfTicket MapTicket(
            ISafeDataReader reader,
            IDictionary<string, string> columns,
            IDictionary<int, string> currencyList,
            string baseCurrency = "",
            long custId = 0)
        {
            var currencyId = reader.GetInt("currencyid");
            return new LrfTicket
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
                PlayerComm = reader.GetDecimal(columns["playerCommCol"]),
                Comm = reader.GetDecimal(columns["commCol"]),
                LicenseeWL = reader.GetDecimal(columns["licenseeWlCol"]),
                Winlost = reader.GetDecimal(columns["winlostCol"]),
                BetEffective = reader.GetDecimal(columns["beteffectiveCol"]),
                Stake = reader.GetDecimal(columns["stakeCol"]),
                ActualStake = reader.GetDecimal(columns["actualStakeCol"]),
                Odds = reader.GetDecimal("odds"),
                MasterOdds = reader.GetDecimal("modds"),
                Handicap1 = reader.GetDecimal("hdp1"),
                Handicap2 = reader.GetDecimal("hdp2"),
                TransDesc = reader.GetString("transdesc"),
                Currency = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),

                BetdaqId = reader.GetLong("betdaqid", -1),
                ExtraInfo = reader.GetString("extrainfo"),
                OriginalStake = reader.GetDecimal(columns["originalStakeCol"], -1),
                OriginalBetType = reader.GetInt("originalbettype"),
                OddsSpread = reader.GetDecimal("oddsspread"),
                SettlementStatus = reader.GetByte("SettlementStatus")
            };
        }

        [CLSCompliant(false)]
        protected internal SortableList<LrfTicket> ReadBetListTicket(
            IDataReader dataReader,
            IDictionary<int, string> currencyList,
            string baseCurrency = "",
            long custId = 0)
        {
            var columns = GetColumns(baseCurrency);
            var tickets = new SortableList<LrfTicket>();

            var reader = this.GetSafeDataReader(dataReader, columns.Select(x => x.Value).ToList());
            while (reader.Read())
            {
                tickets.Add(MapTicket(reader, columns, currencyList, baseCurrency, custId));
            }

            return tickets;
        }

        protected internal virtual ISafeDataReader GetSafeDataReader(IDataReader reader, IEnumerable<string> columns)
        {
            return new SafeDataReader(reader, columns);
        }

        protected static IDictionary<string, string> GetColumns(string baseCurrency = "")
        {
            var columnSuffix = SystemConstants.BaseCurrencyList.Contains(baseCurrency?.ToUpperInvariant()) ? baseCurrency : string.Empty;

            var columns = new[]
             {
                "username", "refno", "matchcode", "status", "betteam", "betstatus", "showtime", "oddstype",
                "nickname", "race", "betfrom", "betcheck", "liveindicator", "isneutral", "sporttype", "bettype",
                "leagueid", "awayid", "homeid", "matchid", "custid", "currencyid", "livehomescore", "liveawayscore",
                "SpecialLevel", "StatusId", "transid", "betid", "winlostdate", "transdate",
                "odds", "modds", "hdp1", "hdp2", "transdesc", "currency", "betdaqid", "extrainfo", "originalbettype",
                "refcode", "wallettype", "oddsspread", "SettlementStatus"
            };
            var result = columns.ToDictionary(x => x, x => x);
            result.Add("playerCommCol", "playercomm" + columnSuffix);
            result.Add("commCol", "comm" + columnSuffix);
            result.Add("licenseeWlCol", "licenseewl" + columnSuffix);
            result.Add("winlostCol", "winlost" + columnSuffix);
            result.Add("beteffectiveCol", "beteffective" + columnSuffix);
            result.Add("stakeCol", "stake" + columnSuffix);
            result.Add("actualStakeCol", "actual_stake" + columnSuffix);
            result.Add("originalStakeCol", "originalstake" + columnSuffix);
            return result;
        }
    }
}