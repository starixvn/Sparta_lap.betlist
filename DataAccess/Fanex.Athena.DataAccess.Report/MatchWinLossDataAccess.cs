// -----------------------------------------------------------------------
// <copyright file="MatchWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using VendorAdapter.Interfaces;

    public class MatchWinLossDataAccess : BaseDataAccess, IMatchWinLossDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public MatchWinLossDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_MatchWinLoss";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("Date", metaModel.Date);
                db.SetParameterValue("MoveBetsDay", _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
                foreach (var item in metaModel.ProductOptions)
                {
                    db.SetParameterValue("show" + item.Code, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    return SetToList(reader, metaModel.BaseCurrency);
                }
            }
        }

        public DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_MatchWinLossDetail";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("FromDate", metaModel.Date);
                db.SetParameterValue("ToDate", metaModel.Date);
                db.SetParameterValue("MoveBetsDay", _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
                db.SetParameterValue("PageSize", metaModel.PageSize);

                foreach (var item in metaModel.ProductOptions)
                {
                    db.SetParameterValue("show" + item.Code, item.Status);
                }

                return db.ExecuteDataSet();
            }
        }

        public IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_MatchWinLossByBettype";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("MatchId", metaModel.MatchId);
                db.SetParameterValue("Date", metaModel.Date);
                db.SetParameterValue("LeagueId", metaModel.LeagueId);
                db.SetParameterValue("MoveBetsDay", _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);

                foreach (var item in metaModel.ProductCodeList.Split(','))
                {
                    db.SetParameterValue("show" + item, true);
                }

                using (var reader = db.ExecuteReader())
                {
                    return SetToList(reader, metaModel.BaseCurrency);
                }
            }
        }

        public IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_MatchWinLossByOutrightParlay";

            using (var db = this.GetObjectDb(UspName))
            {
                var showSb = metaModel.ProductOptions.Any(i => i.Status && i.Code == Product.SB);
                var showBa = metaModel.ProductOptions.Any(i => i.Status && i.Code == Product.BA);
                var showVS2 = metaModel.ProductOptions.Any(i => i.Status && i.Code == Product.VS2);

                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("Date", metaModel.Date);
                db.SetParameterValue("MoveBetsDay", _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
                db.SetParameterValue("showsb", showSb);
                db.SetParameterValue("showba", showBa);
                db.SetParameterValue("showVS2", showVS2);

                using (var reader = db.ExecuteReader())
                {
                    return SetToList(reader, metaModel.BaseCurrency);
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private IList<MatchWinLoss> SetToList(IDataReader reader, string baseCurrency)
        {
            var turnoverColumn = "Turnover" + baseCurrency;
            var winlossColumn = "MemberWinLost" + baseCurrency;
            var custCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;
            var hasCustComm = reader.HasColumn(custCommColumn);
            var hasBetType = reader.HasColumn("bettype");
            var hasProductName = reader.HasColumn("productname");
            var hasLiveIndicator = reader.HasColumn("liveindicator");
            var hasIsNeutral = reader.HasColumn("isneutral");
            var hasKickOffTime = reader.HasColumn("KickoffTime");
            var hasShowTime = reader.HasColumn("showtime");
            var hasSportType = reader.HasColumn("sporttype");
            var dataList = new List<MatchWinLoss>();
            while (reader.Read())
            {
                var betType = hasBetType ? reader.ToInt("bettype") : 0;
                var isLive = hasLiveIndicator && Converter.ToBool(reader["liveindicator"]);
                dataList.Add(new MatchWinLoss
                {
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    MatchCode = reader.ToString("matchcode"),
                    HomeId = reader.ToInt("homeid"),
                    AwayId = reader.ToInt("awayid"),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("betcount"),
                    CustomerWL = reader.ToDecimal(winlossColumn),
                    CustomerComm = hasCustComm ? reader.ToDecimal(custCommColumn) : 0,
                    LicenseeWL = reader.ToDecimal(licenseeWlColumn),
                    BACommission = reader.ToDecimal(commBaColumn),
                    AmtDueToBA = reader.ToDecimal(amtDueToBaComumn),
                    RaceNo = reader.ToString("race"),
                    BetType = betType,
                    BettypeName = _cacheRefAdapter.GetBettype(betType, isLive),
                    ProductName = hasProductName ? reader.ToString("productname") : string.Empty,
                    IsLive = isLive,
                    IsNeutral = hasIsNeutral && Converter.ToBool(reader["isNeutral"]),
                    ShowTime = hasShowTime ? reader.ToString("showtime") : string.Empty,
                    SportType = hasSportType ? reader.ToShort("sporttype") : (short)0,
                    KickOffTime = hasKickOffTime ? reader.ToDateTime("KickoffTime") : DateTime.MinValue
                });
            }

            return dataList;
        }
    }
}