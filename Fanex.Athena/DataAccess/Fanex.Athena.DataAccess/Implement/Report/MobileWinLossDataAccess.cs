// -----------------------------------------------------------------------
// <copyright file="MobileWinLossDataAccess.cs" company="Nexcel Solution">
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
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Mobile win loss data access.
    /// </summary>
    public class MobileWinLossDataAccess : BaseDataAccess, IMobileWinLossDataAccess
    {
        public MobileWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region IMobileWinLossDataAccess Members

        /// <summary>
        ///   Get Data List.
        /// </summary>
        /// <param name="metaModel"> The meta Model. </param>
        /// <param name="summaryTotal"> The total in EUR. </param>
        /// <returns> The result value.</returns>
        public IList<MobileWinLoss> GetMasterData(MobileWinLossMetaModel metaModel, out Dictionary<string, MobileWinLoss> summaryTotal)
        {
            const string UspName = "Lrf_Rpt_MobileWinloss";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel, metaModel.CurrencyId);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion IMobileWinLossDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static IList<MobileWinLoss> GetData(
            IDataReader reader,
            MobileWinLossMetaModel metaModel,
            out Dictionary<string, MobileWinLoss> summaryTotal)
        {
            var dataList = new List<MobileWinLoss>();
            summaryTotal = new Dictionary<string, MobileWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new MobileWinLoss());
            }

            var turnoverColumn = "Turnover" + metaModel.BaseCurrency;
            var winlostComumn = "Winlost" + metaModel.BaseCurrency;
            var playerCommColumn = "PlayerComm" + metaModel.BaseCurrency;
            var licenseeWlColumn = "LicenseeWL" + metaModel.BaseCurrency;
            var commBaColumn = "CommBA" + metaModel.BaseCurrency;
            var commRbColumn = "CommRB" + metaModel.BaseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + metaModel.BaseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + metaModel.BaseCurrency;
            var winLossRbColumn = "WinlossRB" + metaModel.BaseCurrency;

            var index = 1;
            while (reader.Read())
            {
                // Read Item
                var mobile = new MobileWinLoss
                {
                    Index = index,
                    CurrencyName = reader.ToString("currencyName"),
                    TotalTransactions = reader.ToInt("Betcount"),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    CustomerWinLoss = reader.ToDecimal(winlostComumn),
                    CustomerComm = reader.ToDecimal(playerCommColumn),
                    LicenseeWL = reader.ToDecimal(licenseeWlColumn),
                    BACommission = reader.ToDecimal(commBaColumn),
                    RacingRevenue = reader.ToDecimal(winLossRbColumn) + reader.ToDecimal(commRbColumn),
                    AmtDueToBA = reader.ToDecimal(amtDueToBaComumn),
                    AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn),
                    CurrencyId = reader.ToInt("Currency")
                };

                // mobile.Date = level == 0 ? reader.ToDateTime("winlostdate") : metaModel.FromDate;
                mobile.SystemId = reader.ToString("username");
                mobile.CustomerId = reader.ToInt("custid");
                mobile.ExternalId = reader.ToString("nickname");

                dataList.Add(mobile);
                index++;

                // Read Summary
                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].TotalTransactions += reader.ToInt("Betcount");
                    summaryTotal[currency].Turnover += reader.ToDecimal("Turnover" + currency);
                    summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("Winlost" + currency);
                    summaryTotal[currency].CustomerComm += reader.ToDecimal("PlayerComm" + currency);
                    summaryTotal[currency].LicenseeWL += reader.ToDecimal("LicenseeWL" + currency);
                    summaryTotal[currency].BACommission += reader.ToDecimal("CommBA" + currency);
                    summaryTotal[currency].RacingRevenue += reader.ToDecimal("WinlossRB" + currency) + reader.ToDecimal("CommRB" + currency);
                    summaryTotal[currency].AmtDueToBA += reader.ToDecimal("AmtDueToBA" + currency);
                    summaryTotal[currency].AmtDueToRacing += reader.ToDecimal("AmtDueToRB" + currency);
                }
            }

            return dataList;
        }

        /// <summary>
        /// Update USP parameter.
        /// </summary>
        /// <param name="db">Database value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="currencyId">Currency ID value.</param>
        private void UpdateUspParams(ObjectDb db, MobileWinLossMetaModel metaModel, int currencyId)
        {
            currencyId = Math.Max(0, currencyId);

            db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            db.AddInParameter("sitetype", DbType.Byte, metaModel.Site);
            db.AddInParameter("movebetsday", DbType.DateTime, BalanceUp.MoveBetsDay);
            db.AddInParameter("DrawFilter", DbType.Int16, metaModel.DrawFilter);
            db.AddInParameter("excludetest", DbType.Boolean, metaModel.IsExcludeTest);
            db.AddInParameter("CurrencyId", DbType.Int32, currencyId);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        #endregion Private
    }
}