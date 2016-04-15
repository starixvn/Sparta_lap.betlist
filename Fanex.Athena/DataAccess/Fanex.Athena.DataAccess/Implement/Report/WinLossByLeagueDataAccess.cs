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
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
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

        public IEnumerable<WinLossByLeague> GetWinlossByLeagues(WinLossByLeagueMetaModel metaModel)
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
                        return GetMatches(reader, metaModel.BaseCurrency);
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
        public IEnumerable<WinLossByLeague> GetWinlossByLeagueCustomers(
            WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalRows)
        {
            const string UspName = "Lrf_Rpt_WinlossByLeagueDetail";
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
                    db.AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
                    db.AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
                    db.AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
                    db.AddInParameter("showsb", DbType.Boolean, showSb);
                    db.AddInParameter("showba", DbType.Boolean, showBa);
                    db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                    db.AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                    db.AddOutParameter("totalrows", DbType.Int32, int.MaxValue);

                    IEnumerable<WinLossByLeague> dataList = null;

                    using (var reader = db.ExecuteReader())
                    {
                        dataList = GetCustomers(reader, metaModel.BaseCurrency);
                        reader.NextResult();
                        grandTotal = GetGrandTotal(reader, metaModel.BaseCurrency);
                    }

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

        private static IEnumerable<WinLossByLeague> GetMatches(IDataReader reader, string baseCurrency)
        {
            var result = new List<WinLossByLeague>();
            var turnOverColumn = "turnover" + baseCurrency;
            var memberWLColumn = "memberwinlost" + baseCurrency;
            var licenseeWLColumn = "licenseewl" + baseCurrency;
            var comBAColumn = "CommBA" + baseCurrency;
            var amtDueToBAColumn = "amtduetoBa" + baseCurrency;
            var index = 1;
            while (reader.Read())
            {
                var item = new WinLossByLeague
                {
                    Index = index++,
                    TurnOver = reader.ToDecimal(turnOverColumn),
                    CustomerWinLoss = reader.ToDecimal(memberWLColumn),
                    LicenseeWL = reader.ToDecimal(licenseeWLColumn),
                    BACommission = reader.ToDecimal(comBAColumn),
                    BetCount = reader.ToInt("betcount"),
                    AmountDue = reader.ToDecimal(amtDueToBAColumn),
                    HomeId = reader.ToInt("homeid"),
                    AwayId = reader.ToInt("awayid")
                };
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Report data table.</param>
        /// <param name="grandTotalTable">Grand total data table.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="grandTotal">Grand total object.</param>
        /// <returns>The result value.</returns>
        private static IEnumerable<WinLossByLeague> GetCustomers(IDataReader reader, string baseCurrency)
        {
            var result = new List<WinLossByLeague>();

            var index = 1;
            var turnoverCol = "turnover" + baseCurrency;
            var memberWlCol = "memberwinlost" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var bacommCol = "CommBA" + baseCurrency;

            while (reader.Read())
            {
                var hasSystemId = reader.HasColumn("username");
                var hasExternalId = reader.HasColumn("nickname");
                var hasCustomerId = reader.HasColumn("custid");

                var item = new WinLossByLeague
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
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

        private static WinLossByLeague GetGrandTotal(IDataReader reader, string baseCurrency)
        {
            var turnoverCol = "turnover" + baseCurrency;
            var memberWlCol = "memberwinlost" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var bacommCol = "CommBA" + baseCurrency;

            var grandTotal = new WinLossByLeague();
            if (reader.Read())
            {
                grandTotal.TurnOver = reader.ToDecimal(turnoverCol);
                grandTotal.CustomerWinLoss = reader.ToDecimal(memberWlCol);
                grandTotal.LicenseeWL = reader.ToDecimal(licenseeWlCol);
                grandTotal.BACommission = reader.ToDecimal(bacommCol);
                grandTotal.BetCount = reader.ToInt("betcount");
            }

            return grandTotal;
        }

        #endregion Private
    }
}