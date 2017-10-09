// -----------------------------------------------------------------------
// <copyright file="WinLossByLeagueDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Model.Core.ResultModel;

    public class WinLossByLeagueDataAccess : BaseDataAccess, IWinLossByLeagueDataAccess
    {
        public IEnumerable<WinLossByLeague> GetSummary(WinLossByLeagueMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinlostByLeague";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status);
                    var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status);

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
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetMatches(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        public ReportResultModel<WinLossByLeague> GetCustomers(WinLossByLeagueMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinlossByLeagueDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status);
                    var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status);

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
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddOutParameter("totalrows", DbType.Int32, int.MaxValue);

                    IEnumerable<WinLossByLeague> dataList = null;
                    WinLossByLeague totalData;

                    using (var reader = db.ExecuteReader())
                    {
                        dataList = GetCustomers(reader, metaModel.BaseCurrency);
                        reader.NextResult();
                        totalData = GetTotalData(reader, metaModel.BaseCurrency);
                    }

                    var totalRows = Converter.ToInt(db.GetParameterValue("totalrows"));
                    return new ReportResultModel<WinLossByLeague>
                    {
                        DataList = dataList,
                        TotalData = totalData,
                        TotalRows = totalRows
                    };
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

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

        private static WinLossByLeague GetTotalData(IDataReader reader, string baseCurrency)
        {
            var turnoverCol = "turnover" + baseCurrency;
            var memberWlCol = "memberwinlost" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var bacommCol = "CommBA" + baseCurrency;

            var totalData = new WinLossByLeague();
            if (reader.Read())
            {
                totalData.TurnOver = reader.ToDecimal(turnoverCol);
                totalData.CustomerWinLoss = reader.ToDecimal(memberWlCol);
                totalData.LicenseeWL = reader.ToDecimal(licenseeWlCol);
                totalData.BACommission = reader.ToDecimal(bacommCol);
                totalData.BetCount = reader.ToInt("betcount");
            }

            return totalData;
        }

        #endregion Private
    }
}