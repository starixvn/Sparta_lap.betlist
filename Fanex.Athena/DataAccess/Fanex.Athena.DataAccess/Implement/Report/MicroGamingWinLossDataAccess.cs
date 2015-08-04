// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// MicroGaming win loss data access.
    /// </summary>
    public class MicroGamingWinLossDataAccess : BaseDataAccess, IMicroGamingWinLossDataAccess
    {
        public MicroGamingWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        public SortableList<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel, out IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            var uspName = "LRF_Rpt_MicroGamingWinloss";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel);
                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<MicroGamingWinLoss> GetData(
            IDataReader reader,
            MicroGamingWinLossMetaModel metaModel,
            out IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            var result = new SortableList<MicroGamingWinLoss>();
            
            summaryTotal = new Dictionary<string, MicroGamingWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new MicroGamingWinLoss());
            }

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var index = 1;
            while (reader.Read())
            {
                var item = new MicroGamingWinLoss
                {
                    Index = index++,
                    TypeID = reader.ToInt("typeid"),
                    TypeName = reader.ToString("typename"),
                    CurrencyID = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    Date = ConvertHelper.ToDateTime(reader.ToString("winlostdate"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item);

                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                    summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                    summaryTotal[currency].LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                    summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                }
            }

            return result;
        }

        private void UpdateUspParams(ObjectDb db, MicroGamingWinLossMetaModel metaModel)
        {
            var currencyId = metaModel.CurrencyId < 0 ? 0 : metaModel.CurrencyId;           

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("currencyid", DbType.Int32, currencyId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
        }

        #endregion Private
    }
}