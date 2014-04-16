// -----------------------------------------------------------------------
// <copyright file="MatchWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
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
        /// Set to list.
        /// </summary>
        /// <param name="reader">Reader  value.</param>
        /// <param name="baseCurrency">Base currency  value.</param>
        /// <returns> The result value.</returns>
        public static IList<MatchWinLoss> SetToList(IDataReader reader, string baseCurrency)
        {
            var dataList = new List<MatchWinLoss>();
            var turnoverColumn = "Turnover" + baseCurrency;
            var winlossColumn = "MemberWinLost" + baseCurrency;
            var custCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var commRbColumn = "CommRB" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + baseCurrency;
            var winLossRbColumn = "WinlossRB" + baseCurrency;

            var custCommExisted = reader.FieldExists(custCommColumn);
            var betTypeExisted = reader.FieldExists("bettype");
            var productNameExisted = reader.FieldExists("productname");
            var liveIndicatorExisted = reader.FieldExists("liveindicator");
            var isNeutralExisted = reader.FieldExists("isneutral");
            var kickofTime = reader.FieldExists("KickoffTime");

            while (reader.Read())
            {
                var matchWinLoss = new MatchWinLoss();
                matchWinLoss.LeagueId = reader.ToInt("leagueid");
                matchWinLoss.MatchId = reader.ToInt("matchid");
                matchWinLoss.MatchCode = reader.ToString("matchcode");
                matchWinLoss.HomeId = reader.ToInt("homeid");
                matchWinLoss.AwayId = reader.ToInt("awayid");
                matchWinLoss.Turnover = reader.ToDecimal(turnoverColumn);
                matchWinLoss.CustomerWl = reader.ToDecimal(winlossColumn);
                matchWinLoss.CustomerComm = custCommExisted ? reader.ToDecimal(custCommColumn) : 0;
                matchWinLoss.LicenseeWl = reader.ToDecimal(licenseeWlColumn);
                matchWinLoss.BaCommission = reader.ToDecimal(commBaColumn);
                matchWinLoss.RacingRevenue = reader.ToDecimal(winLossRbColumn) + reader.ToDecimal(commRbColumn);
                matchWinLoss.AmtDueToBa = reader.ToDecimal(amtDueToBaComumn);
                matchWinLoss.AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn);
                matchWinLoss.RaceNo = reader.ToString("race");
                matchWinLoss.BetType = betTypeExisted ? reader.ToInt("bettype") : 0;
                matchWinLoss.ProductName = productNameExisted ? reader.ToString("productname") : string.Empty;
                matchWinLoss.IsLive = liveIndicatorExisted && Converter.ToBool(reader["liveindicator"]);
                matchWinLoss.IsNeutral = isNeutralExisted && Converter.ToBool(reader["isNeutral"]);
                matchWinLoss.ShowTime = reader.FieldExists("showtime") ? reader.ToString("showtime") : string.Empty;
                matchWinLoss.SportType = reader.FieldExists("sporttype") ? reader.ToShort("sporttype") : (short)0;
                matchWinLoss.KickofTime = kickofTime ? reader.ToDateTime("KickoffTime") : DateTime.MinValue;
                dataList.Add(matchWinLoss);
            }

            return dataList;
        }

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
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                    foreach (var item in metaModel.ProductOptions)
                    {
                        db.AddInParameter("show" + item.Code.ToLower(), DbType.Boolean, item.Status);
                    }

                    using (var reader = db.ExecuteReader())
                    {
                        return SetToList(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (SqlException sqlException)
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
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                    db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);

                    foreach (var item in metaModel.ProductOptions)
                    {
                        db.AddInParameter("show" + item.Code.ToLower(), DbType.Boolean, item.Status);
                    }

                    return db.ExecuteDataSet();
                }
            }
            catch (SqlException sqlException)
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
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                    db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);

                    foreach (var item in metaModel.ProductOptions)
                    {
                        db.AddInParameter("show" + item.Code.ToLower(), DbType.Boolean, item.Status);
                    }

                    using (var reader = db.ExecuteReader())
                    {
                        return SetToList(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (SqlException sqlException)
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
                var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.Sb && i.Status == true);
                var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.Ba && i.Status == true);
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
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
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get out right parlay data set.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public DataSet GetOutrightParlayDataSet(MatchWinLossMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_MatchWinLossByOutrightParlay";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);

                    return db.ExecuteDataSet();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        #endregion IMatchWinLossDataAccess Members
    }
}