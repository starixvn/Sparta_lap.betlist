namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Core.Configuration.Interfaces;
    using Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;

    public class SubsidiaryPerformanceDataAccess : BaseDataAccess, ISubsidiaryPerformanceDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public SubsidiaryPerformanceDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public async Task<IEnumerable<SubsidiaryPerformance>> GetData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            var dataList = new List<SubsidiaryPerformance>();

            using (var objectDb = GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = await objectDb.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var data = MapData(reader, currencyList, metaModel.ConvertAll);

                        dataList.Add(data);
                    }
                }
            }

            return dataList;
        }

        public async Task<ReportResultModel<SubsidiaryPerformanceDetail>> GetDetailData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinlossDetail";
            var dataList = new List<SubsidiaryPerformanceDetail>();
            var totalList = new Dictionary<string, SubsidiaryPerformanceDetail>();

            using (var objectDb = GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = await objectDb.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var baseColumnSuffix = metaModel.ConvertAll ? "Base" : string.Empty;

                    while (reader.Read())
                    {
                        var data = MapDetailData(reader, currencyList, baseColumnSuffix);
                        data.SystemId = reader["username"].ToString();
                        data.ExternalId = reader["nickname"].ToString();
                        dataList.Add(data);

                        if (metaModel.ConvertAll)
                        {
                            CalculateDetailTotal(data, totalList);
                        }
                        else
                        {
                            CalculateDetailTotal(data.CurrencyName, reader, totalList);
                        }
                    }
                }
            }

            return new ReportResultModel<SubsidiaryPerformanceDetail>
            {
                DataList = dataList,
                SummaryTotal = totalList,
                TotalData = CalculateDetailGrandTotal(metaModel.BaseCurrency.ToUpperInvariant(), totalList.Values)
            };
        }

        private SubsidiaryPerformance MapData(IDataReader reader, IDictionary<int, string> currencyList, bool isConvert)
        {
            var currencyId = Convert.ToInt32(reader["Currency"]);
            var result = new SubsidiaryPerformance
            {
                ProductCode = reader["ProductCode"].ToString(),
                SubsidiaryId = Convert.ToInt32(reader["recommend"]),
                CurrencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),
                CurrencyId = currencyId,
                BaseCurrencyData = new SubsidiaryPerformanceBase
                {
                    BetCount = Convert.ToInt32(reader["BetCount"]),
                    Turnover = Convert.ToDecimal(reader["TurnoverBase"]),
                    CustomerWinLoss = Convert.ToDecimal(reader["WinlostBase"]),
                    SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWLBase"])
                }
            };
            result.BetCount = result.BaseCurrencyData.BetCount;

            if (isConvert)
            {
                result.Turnover = result.BaseCurrencyData.Turnover;
                result.CustomerWinLoss = result.BaseCurrencyData.CustomerWinLoss;
                result.SubsidiaryWinLoss = result.BaseCurrencyData.SubsidiaryWinLoss;
            }
            else
            {
                result.Turnover = Convert.ToDecimal(reader["Turnover"]);
                result.CustomerWinLoss = Convert.ToDecimal(reader["Winlost"]);
                result.SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWL"]);
            }

            return result;
        }

        private SubsidiaryPerformanceDetail MapDetailData(IDataReader reader, IDictionary<int, string> currencyList, string baseColumnSuffix = "Base")
        {
            var currencyId = Convert.ToInt32(reader["Currency"]);
            var result = new SubsidiaryPerformanceDetail
            {
                SystemId = reader["username"].ToString(),
                ExternalId = reader["nickname"].ToString(),
                CurrencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),
                CurrencyId = currencyId,
                BetCount = Convert.ToInt32(reader["BetCount"]),
                Turnover = Convert.ToDecimal(reader["Turnover" + baseColumnSuffix]),
                CustomerWinLoss = Convert.ToDecimal(reader["Winlost" + baseColumnSuffix]),
                SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWL" + baseColumnSuffix])
            };

            return result;
        }

        private void CalculateDetailTotal(string currencyName, IDataReader reader, IDictionary<string, SubsidiaryPerformanceDetail> totalList)
        {
            if (!totalList.ContainsKey(currencyName))
            {
                totalList.Add(currencyName, new SubsidiaryPerformanceDetail { CurrencyName = currencyName });
            }

            var total = totalList[currencyName];

            total.BetCount += Convert.ToInt32(reader["BetCount"]);
            total.Turnover += Convert.ToDecimal(reader["TurnoverBase"]);
            total.CustomerWinLoss += Convert.ToDecimal(reader["WinlostBase"]);
            total.SubsidiaryWinLoss += Convert.ToDecimal(reader["LicenseeWLBase"]);
        }

        private void CalculateDetailTotal(SubsidiaryPerformanceDetail detail, IDictionary<string, SubsidiaryPerformanceDetail> totalList)
        {
            if (!totalList.ContainsKey(detail.CurrencyName))
            {
                totalList.Add(detail.CurrencyName, new SubsidiaryPerformanceDetail { CurrencyName = detail.CurrencyName });
            }

            var total = totalList[detail.CurrencyName];

            total.BetCount += detail.BetCount;
            total.Turnover += detail.Turnover;
            total.CustomerWinLoss += detail.CustomerWinLoss;
            total.SubsidiaryWinLoss += detail.SubsidiaryWinLoss;
        }

        private SubsidiaryPerformanceDetail CalculateDetailGrandTotal(string baseCurrency, IEnumerable<SubsidiaryPerformanceDetail> total)
        {
            return new SubsidiaryPerformanceDetail
            {
                CurrencyName = baseCurrency,
                BetCount = total.Sum(x => x.BetCount),
                Turnover = total.Sum(x => x.Turnover),
                CustomerWinLoss = total.Sum(x => x.CustomerWinLoss),
                SubsidiaryWinLoss = total.Sum(x => x.SubsidiaryWinLoss)
            };
        }

        private void AssignParameter(IObjectDb objectDb, SubsidiaryPerformanceMetaModel metaModel)
        {
            objectDb.SetParameterValue("FromDate", metaModel.FromDate);
            objectDb.SetParameterValue("ToDate", metaModel.ToDate);
            objectDb.SetParameterValue("MoveBetsDay", _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            objectDb.SetParameterValue("ProductList", metaModel.ProductCodeList);
            objectDb.SetParameterValue("IncludeRaw", metaModel.IncludeRaw);
            objectDb.SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
        }
    }
}