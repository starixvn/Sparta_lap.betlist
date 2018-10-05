namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Core.Extensions;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Interfaces;
    using Model.Core.ResultModel;
    using Session.Interfaces;

    public class SpecialLicenseeWinLossDataAccess : BaseDataAccess, ISpecialLicenseeWinLossDataAccess
    {
        public SpecialLicenseeWinLossDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public ReportResultModel<LicenseeWinLoss> GetData(SpecialLicenseeWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LicWinloss_FromAPI";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        return ReadData(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static IDictionary<string, string> CreateGroupedColumns(IDataReader reader, string columnName, SpecialLicenseeWinLossMetaModel metaModel)
        {
            var cols = new Dictionary<string, string>();

            if (reader.FieldCount > 0)
            {
                foreach (var code in metaModel.ProductCodeList)
                {
                    var columnBase = columnName + code + metaModel.BaseCurrency;

                    if (reader.HasColumn(columnBase))
                    {
                        cols.Add(columnBase, code);
                    }
                }
            }

            return cols;
        }

        private static IDictionary<string, LicenseeWinLoss> CreateSummaryTotal(string[] baseCurrencies)
        {
            var summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            foreach (var currency in baseCurrencies)
            {
                summaryTotal.Add(
                    currency,
                     new LicenseeWinLoss
                     {
                         LicenseeWL = new Dictionary<string, decimal>(),
                         TurnOver = new Dictionary<string, decimal>()
                     });
            }

            return summaryTotal;
        }

        private static void ReadSummaryTotal(
            IDataReader reader,
            SpecialLicenseeWinLossMetaModel metaModel,
            IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            foreach (var currency in metaModel.BaseCurrencies)
            {
                var summaryItem = summaryTotal[currency];
                summaryItem.CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                summaryItem.TotalTransaction += reader.ToInt("betcount");

                decimal temp;
                var licenseeWlSummary = new Dictionary<string, decimal>();
                var turnOverSummary = new Dictionary<string, decimal>();
                foreach (var code in metaModel.ProductCodeList)
                {
                    var licenseeWlColumnName = "LicenseeWL" + code + currency;
                    var turnOverColumnName = "turnover" + code + currency;

                    if (reader.HasColumn(licenseeWlColumnName))
                    {
                        summaryItem.LicenseeWL.TryGetValue(code, out temp);
                        temp += reader.ToDecimal(licenseeWlColumnName);
                        licenseeWlSummary.Add(code, temp);
                    }

                    if (reader.HasColumn(turnOverColumnName))
                    {
                        summaryItem.TurnOver.TryGetValue(code, out temp);
                        temp += reader.ToDecimal(turnOverColumnName);
                        turnOverSummary.Add(code, temp);
                    }
                }

                summaryItem.LicenseeWL = licenseeWlSummary;
                summaryItem.TurnOver = turnOverSummary;
            }
        }

        private static IDictionary<string, decimal> ReadTurnoverList(IDataReader reader, IDictionary<string, string> turnoverColumns)
        {
            var turnOver = new Dictionary<string, decimal>();
            foreach (var column in turnoverColumns)
            {
                turnOver.Add(column.Value, reader.ToDecimal(column.Key));
            }

            return turnOver;
        }

        private static IDictionary<string, decimal> ReadLicenseeWinLossList(IDataReader reader, IDictionary<string, string> licenseeWLColumns)
        {
            var licenseeWl = new Dictionary<string, decimal>();

            foreach (var column in licenseeWLColumns)
            {
                licenseeWl.Add(column.Value, reader.ToDecimal(column.Key));
            }

            return licenseeWl;
        }

        private static IEnumerable<LicenseeWinLoss> CalculateTotal(IEnumerable<LicenseeWinLoss> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<LicenseeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new LicenseeWinLoss
            {
                CurrencyName = g.Key,
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                TurnOver = g.SelectMany(k => k.TurnOver).GroupBy(j => j.Key).ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                LicenseeWL = g.SelectMany(k => k.LicenseeWL).GroupBy(j => j.Key).ToDictionary(p => p.Key, p => p.Sum(j => j.Value))
            }).ToList();
        }

        private static void UpdateUspParams(IObjectDb db, SpecialLicenseeWinLossMetaModel metaModel)
        {
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            db.AddInParameter("reporttype", DbType.Byte, metaModel.ReportBy);
            db.AddInParameter("productCodes", DbType.String, string.Join(",", metaModel.ProductCodeList));
            db.AddInParameter("currencylist", DbType.String, metaModel.CurrencyListXml);
            db.AddInParameter("siteName", DbType.String, metaModel.SiteName);
        }

        private ReportResultModel<LicenseeWinLoss> ReadData(
            IDataReader reader,
            SpecialLicenseeWinLossMetaModel metaModel)
        {
            var dataList = new List<LicenseeWinLoss>();
            var summaryTotal = CreateSummaryTotal(metaModel.BaseCurrencies);
            var licenseeWLColumns = CreateGroupedColumns(reader, "licenseeWL", metaModel);
            var turnoverColumns = CreateGroupedColumns(reader, "turnover", metaModel);

            while (reader.Read())
            {
                var currencyId = reader.ToInt("currencyid");
                var item = new LicenseeWinLoss
                {
                    CurrencyId = currencyId,
                    CurrencyName = GetCurrencyName(currencyId),
                    CustomerWinLoss = reader.ToDecimal("winlost" + metaModel.BaseCurrency),
                    LicenseeWL = ReadLicenseeWinLossList(reader, licenseeWLColumns),
                    TurnOver = ReadTurnoverList(reader, turnoverColumns),
                    TotalTransaction = reader.ToInt("betcount"),
                    Date = reader.ToDateTime("reporttype")
                };

                dataList.Add(item);
                ReadSummaryTotal(reader, metaModel, summaryTotal);
            }

            return new ReportResultModel<LicenseeWinLoss>
            {
                DataList = dataList,
                TotalDataList = CalculateTotal(dataList),
                SummaryTotal = summaryTotal
            };
        }
    }
}