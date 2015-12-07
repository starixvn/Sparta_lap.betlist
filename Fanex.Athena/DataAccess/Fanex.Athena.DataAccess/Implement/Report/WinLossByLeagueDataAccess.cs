// -----------------------------------------------------------------------
// <copyright file="WinLossByLeagueDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections;
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
    /// Win loss by product data access.
    /// </summary>
    public class WinLossByLeagueDataAccess : BaseDataAccess, IWinLossByLeagueDataAccess
    {        
        public WinLossByLeagueDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region IWinLossByLeagueDataAccess Members

        /// <summary>
        /// Get win loss by product.
        /// </summary>
        /// <param name="metaModel"> Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<WinLossByLeague> GetWinlossByLeagues(WinLossByLeagueMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinlostByLeague";
            try
            {                
                using (var db = new ObjectDb(UspName))
                {
                    var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status == true);
                    var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status == true);

                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("odds", DbType.Decimal, metaModel.Odds);
                    db.AddInParameter("leagueid", DbType.Int32, metaModel.League);
                    db.AddInParameter("matchid", DbType.Int32, metaModel.Match);
                    db.AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
                    db.AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
                    db.AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
                    db.AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
                    db.AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
                    db.AddInParameter("showsb", DbType.Boolean, showSb);
                    db.AddInParameter("showba", DbType.Boolean, showBa);

                    using (var reader = db.ExecuteReader())
                    {             
                        return GetMatches(reader, metaModel);                        
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        /// <summary>
        /// Get win loss by product customer.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="grandTotal">Grand total object.</param>
        /// <param name="totalRows">Total rows.</param>
        /// <returns>The result value.</returns>
        public IList<WinLossByLeague> GetWinlossByLeagueCustomers(
            WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalRows)
        {
            const string UspName = "Lrf_Rpt_WinlossByLeagueDetail";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status == true);
                    var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status == true);

                    IList<WinLossByLeague> dataList = null;

                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("odds", DbType.Decimal, metaModel.Odds);
                    db.AddInParameter("leagueid", DbType.Int32, metaModel.League);
                    db.AddInParameter("matchid", DbType.Int32, metaModel.Match);
                    db.AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);                    
                    db.AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
                    db.AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
                    db.AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
                    db.AddInParameter("showsb", DbType.Boolean, showSb);
                    db.AddInParameter("showba", DbType.Boolean, showBa);
                    db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                    db.AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                    db.AddOutParameter("totalrows", DbType.Int32, int.MaxValue);

                    var dataSet = db.ExecuteDataSet();
                    var reportTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    var grandTotalTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    dataList = GetCustomers(reportTable, grandTotalTable, metaModel, out grandTotal);
                   
                    totalRows = Converter.ToInt(db.GetParameterValue("totalrows")); 
                    
                    return dataList;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        #endregion IWinLossByLeagueDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        private static IList<WinLossByLeague> GetMatches(
            IDataReader reader,
            WinLossByLeagueMetaModel metaModel)
        {            
            var result = new List<WinLossByLeague>();

            var index = 1;
            while (reader.Read())
            {                
                var homeid = reader.ToInt("homeid");
                var awayid = reader.ToInt("awayid");

                var item = new WinLossByLeague
                    {             
                        Index = index++,
                        TurnOver = reader.ToDecimal("turnover" + metaModel.BaseCurrency),
                        CustomerWinLoss = reader.ToDecimal("memberwinlost" + metaModel.BaseCurrency),                                                
                        LicenseeWL = reader.ToDecimal("licenseewl" + metaModel.BaseCurrency),
                        BACommission = reader.ToDecimal("CommBA" + metaModel.BaseCurrency),
                        BetCount = reader.ToInt("betcount"),
                        AmountDue = reader.ToDecimal("amtduetoBa" + metaModel.BaseCurrency),
                        HomeId = homeid,
                        AwayId = awayid
                    };                
                result.Add(item);               
            }
            
            return result;
        }

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reportTable">Report data table.</param>
        /// <param name="grandTotalTable">Grand total data table.</param>
        /// <param name="metaModel">Meta model value.</param>          
        /// <param name="grandTotal">Grand total object.</param>          
        /// <returns>The result value.</returns>
        private static IList<WinLossByLeague> GetCustomers(
            DataTable reportTable,
            DataTable grandTotalTable,
            WinLossByLeagueMetaModel metaModel,
            out WinLossByLeague grandTotal)
        {
            var result = new List<WinLossByLeague>();

            var index = 1;
            var turnoverCol = "turnover" + metaModel.BaseCurrency;
            var memberWlCol = "memberwinlost" + metaModel.BaseCurrency;
            var licenseeWlCol = "licenseewl" + metaModel.BaseCurrency;
            var bacommCol = "CommBA" + metaModel.BaseCurrency;

            grandTotal = new WinLossByLeague();

            foreach (DataRow row in grandTotalTable.Rows)
            {
                grandTotal.TurnOver = Converter.ToDecimal(row[turnoverCol]);
                grandTotal.CustomerWinLoss = Converter.ToDecimal(row[memberWlCol]);
                grandTotal.LicenseeWL = Converter.ToDecimal(row[licenseeWlCol]);
                grandTotal.BACommission = Converter.ToDecimal(row[bacommCol]);
                grandTotal.BetCount = Converter.ToInt(row["betcount"]);                
            }
            
            var reader = reportTable.CreateDataReader();
            while (reader.Read())
            {
                var systemIdOrdinal = reader.TryGetOrdinal("username");
                var externalIdOrdinal = reader.TryGetOrdinal("nickname");
                var customerIdOrdinal = reader.TryGetOrdinal("custid");

                var item = new WinLossByLeague
                {
                    Index = index++,
                    SystemId = systemIdOrdinal >= 0 ? reader.ToString("username") : string.Empty,
                    ExternalId = externalIdOrdinal >= 0 ? reader.ToString("nickname") : string.Empty,
                    CustomerId = customerIdOrdinal >= 0 ? reader.ToInt("custid") : 0,
                    TurnOver = reader.ToDecimal(turnoverCol),
                    CustomerWinLoss = reader.ToDecimal(memberWlCol),
                    LicenseeWL = reader.ToDecimal(licenseeWlCol),    
                    BACommission = reader.ToDecimal(bacommCol),
                    BetCount = reader.ToInt("betcount")                                      
                };
                result.Add(item);
            }

            return result;
        }
        
        #endregion Private
    }
}