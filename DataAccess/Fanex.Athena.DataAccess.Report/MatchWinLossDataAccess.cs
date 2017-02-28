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
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class MatchWinLossDataAccess : BaseDataAccess, IMatchWinLossDataAccess
    {
        public MatchWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region IMatchWinLossDataAccess Members

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel)
        {
            string uspName = "Lrf_Rpt_MatchWinLoss";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                    foreach (var item in metaModel.ProductOptions)
                    {
                        db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                    }

                    using (var reader = db.ExecuteReader())
                    {
                        return SetToList(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get detail data set.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_MatchWinLossDetail";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                    db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);

                    foreach (var item in metaModel.ProductOptions)
                    {
                        db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                    }

                    return db.ExecuteDataSet();
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get detail data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_MatchWinLossByBettype";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                    db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);

                    foreach (var item in metaModel.ProductOptions)
                    {
                        db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                    }

                    using (var reader = db.ExecuteReader())
                    {
                        return SetToList(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get out right parlay list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_MatchWinLossByOutrightParlay";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status == true);
                    var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status == true);

                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                    db.AddInParameter("showsb", DbType.Boolean, showSb);
                    db.AddInParameter("showba", DbType.Boolean, showBa);

                    using (var reader = db.ExecuteReader())
                    {
                        return SetToList(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        #endregion IMatchWinLossDataAccess Members

        /// <summary>
        /// Set to list.
        /// </summary>
        /// <param name="reader">Reader  value.</param>
        /// <param name="baseCurrency">Base currency  value.</param>
        /// <returns> The result value.</returns>
        private static IList<MatchWinLoss> SetToList(IDataReader reader, string baseCurrency)
        {
            var turnoverColumn = "Turnover" + baseCurrency;
            var winlossColumn = "MemberWinLost" + baseCurrency;
            var custCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var commRbColumn = "CommRB" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + baseCurrency;
            var winLossRbColumn = "WinlossRB" + baseCurrency;
            var hasCustComm = reader.HasColumn(custCommColumn);
            var hasBetType = reader.HasColumn("bettype");
            var hasProductName = reader.HasColumn("productname");
            var hasLiveIndicator = reader.HasColumn("liveindicator");
            var hasIsNeutral = reader.HasColumn("isneutral");
            var hasKickofTime = reader.HasColumn("KickoffTime");
            var hasShowTime = reader.HasColumn("showtime");
            var hasSportType = reader.HasColumn("sporttype");

            var dataList = new List<MatchWinLoss>();
            while (reader.Read())
            {
                var matchWinLoss = new MatchWinLoss();
                matchWinLoss.LeagueId = reader.ToInt("leagueid");
                matchWinLoss.MatchId = reader.ToInt("matchid");
                matchWinLoss.MatchCode = reader.ToString("matchcode");
                matchWinLoss.HomeId = reader.ToInt("homeid");
                matchWinLoss.AwayId = reader.ToInt("awayid");
                matchWinLoss.Turnover = reader.ToDecimal(turnoverColumn);
                matchWinLoss.TotalTransaction = reader.ToInt("betcount");
                matchWinLoss.CustomerWL = reader.ToDecimal(winlossColumn);
                matchWinLoss.CustomerComm = hasCustComm ? reader.ToDecimal(custCommColumn) : 0;
                matchWinLoss.LicenseeWL = reader.ToDecimal(licenseeWlColumn);
                matchWinLoss.BACommission = reader.ToDecimal(commBaColumn);
                matchWinLoss.RacingRevenue = reader.ToDecimal(winLossRbColumn) + reader.ToDecimal(commRbColumn);
                matchWinLoss.AmtDueToBA = reader.ToDecimal(amtDueToBaComumn);
                matchWinLoss.AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn);
                matchWinLoss.RaceNo = reader.ToString("race");
                matchWinLoss.BetType = hasBetType ? reader.ToInt("bettype") : 0;
                matchWinLoss.ProductName = hasProductName ? reader.ToString("productname") : string.Empty;
                matchWinLoss.IsLive = hasLiveIndicator && Converter.ToBool(reader["liveindicator"]);
                matchWinLoss.IsNeutral = hasIsNeutral && Converter.ToBool(reader["isNeutral"]);
                matchWinLoss.ShowTime = hasShowTime ? reader.ToString("showtime") : string.Empty;
                matchWinLoss.SportType = hasSportType ? reader.ToShort("sporttype") : (short)0;
                matchWinLoss.KickofTime = hasKickofTime ? reader.ToDateTime("KickoffTime") : DateTime.MinValue;
                dataList.Add(matchWinLoss);
            }

            return dataList;
        }
    }
}