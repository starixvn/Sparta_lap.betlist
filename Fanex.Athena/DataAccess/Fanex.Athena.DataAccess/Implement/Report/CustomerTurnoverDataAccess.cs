// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Customer turnover data access.
    /// </summary>
    public class CustomerTurnoverDataAccess : BaseDataAccess, ICustTurnoverDataAccess
    {
        public CustomerTurnoverDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region ICustTurnoverDataAccess Members

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="total">The total data.</param>
        /// <param name="totalRow">The total rows.</param>
        /// <returns>The result value.</returns>
        public IList<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode, out CustTurnover total, out int totalRow)
        {
            total = new CustTurnover();
            totalRow = 0;
            IList<CustTurnover> result = new List<CustTurnover>();
            using (var db = new ObjectDb("Lrf_Rpt_CustomerTurnOver"))
            {
                AddParameters(metaModel, languageCode, db);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var turnover = reader.ToDecimal("Turnover");
                        result.Add(new CustTurnover
                        {
                            // Get turnover row to list item
                            ReportBy = ConvertHelper.ToDateTime(reader.ToString("ReportBy"), metaModel.ReportBy == ReportByType.Monthly),
                            UserName = reader.ToString("username"),
                            Currency = reader.ToString("currency"),
                            SportId = reader.ToInt("sporttype"),
                            TotalTransactions = reader.ToInt("BetCount"),
                            Bettype = reader.ToString("Bettype"),
                            Turnover = turnover,
                            TotalTurnover = turnover,
                            CustomerWinLoss = reader.ToDecimal("Winlost"),
                            CustomerComm = reader.ToDecimal("PlayerComm"),
                        });
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            /*read total data*/
                            total.TotalTransactions = reader.ToInt("SumBetCount");
                            total.Turnover = reader.ToDecimal("SumTurnover");
                            total.TotalTurnover = reader.ToDecimal("SumTurnover");
                            total.CustomerWinLoss = reader.ToDecimal("SumWinlost");
                            total.CustomerComm = reader.ToDecimal("SumComm");
                            totalRow = reader.ToInt("TotalRows");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Add parameter.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="db">The database object.</param>
        private void AddParameters(CustomerTurnoverMetaModel metaModel, string languageCode, ObjectDb db)
        {
            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("odds", DbType.Decimal, metaModel.Odds);
            db.AddInParameter("reportby", DbType.Int32, metaModel.ReportBy);
            db.AddInParameter("username", DbType.String, string.IsNullOrWhiteSpace(metaModel.Username) ? null : metaModel.Username);
            db.AddInParameter("movebetsday", DbType.DateTime, BalanceUp.MoveBetsDay);
            db.AddInParameter("balanceupday", DbType.DateTime, BalanceUp.BalanceUpDay);
            db.AddInParameter("Langkey", DbType.String, languageCode);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("IsExportExcel", DbType.Boolean, metaModel.IsExportExcel);
            db.AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
            db.AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
            db.AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
            db.AddInParameter("Product", DbType.String, metaModel.ProductParameters);
            db.AddInParameter("Bettype", DbType.String, metaModel.BettypeParameters);
        }

        #endregion ICustTurnoverDataAccess Members
    }
}