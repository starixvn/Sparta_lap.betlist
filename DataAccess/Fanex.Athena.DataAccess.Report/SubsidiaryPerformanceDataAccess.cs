namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using Session.Interfaces;

    public class SubsidiaryPerformanceDataAccess : BaseDataAccess, ISubsidiaryPerformanceDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public SubsidiaryPerformanceDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public SubsidiaryPerformanceDataAccess(ISessionStorage sessionStorage, IBalanceUpDataAccess balanceUpDataAccess)
            : base(null, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public IEnumerable<SubsidiaryPerformance> GetData(SubsidiaryPerformanceMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            var dataList = new List<SubsidiaryPerformance>();

            using (var objectDb = GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = MapData(reader, metaModel.ConvertAll);

                        dataList.Add(data);
                    }
                }
            }

            return dataList;
        }

        public ReportResultModel<SubsidiaryPerformanceDetail> GetDetailData(SubsidiaryPerformanceMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinlossDetail";
            var dataList = new List<SubsidiaryPerformanceDetail>();
            var totalList = new Dictionary<string, SubsidiaryPerformanceDetail>();

            using (var objectDb = GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    var baseColumnSuffix = metaModel.ConvertAll ? "Base" : string.Empty;

                    while (reader.Read())
                    {
                        var data = MapDetailData(reader, baseColumnSuffix);
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
                TotalData = CalculateDetailGrandTotal(totalList.Values)
            };
        }

        private SubsidiaryPerformance MapData(IDataReader reader, bool isConvert)
        {
            var currencyId = Convert.ToInt32(reader["Currency"]);
            var result = new SubsidiaryPerformance
            {
                ProductCode = reader["ProductCode"].ToString(),
                SubsidiaryId = Convert.ToInt32(reader["recommend"]),
                CurrencyName = GetCurrencyName(currencyId),
                CurrencyId = currencyId,
                BaseCurrencyData = new SubsidiaryPerformanceBase
                {
                    BetCount = Convert.ToInt32(reader["BetCount"]),
                    Turnover = Convert.ToDecimal(reader["TurnoverBase"]),
                    CustomerWinLoss = Convert.ToDecimal(reader["WinlostBase"]),
                    SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWLBase"]),
                    SubsidiaryComm = Convert.ToDecimal(reader["SubsidiaryCommBase"])
                }
            };
            result.BetCount = result.BaseCurrencyData.BetCount;

            if (isConvert)
            {
                result.Turnover = result.BaseCurrencyData.Turnover;
                result.CustomerWinLoss = result.BaseCurrencyData.CustomerWinLoss;
                result.SubsidiaryWinLoss = result.BaseCurrencyData.SubsidiaryWinLoss;
                result.SubsidiaryComm = result.BaseCurrencyData.SubsidiaryComm;
            }
            else
            {
                result.Turnover = Convert.ToDecimal(reader["Turnover"]);
                result.CustomerWinLoss = Convert.ToDecimal(reader["Winlost"]);
                result.SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWL"]);
                result.SubsidiaryComm = Convert.ToDecimal(reader["SubsidiaryComm"]);
            }

            return result;
        }

        private SubsidiaryPerformanceDetail MapDetailData(IDataReader reader, string baseColumnSuffix = "Base")
        {
            var currencyId = Convert.ToInt32(reader["Currency"]);
            var result = new SubsidiaryPerformanceDetail
            {
                SystemId = reader["username"].ToString(),
                ExternalId = reader["nickname"].ToString(),
                CurrencyName = GetCurrencyName(currencyId),
                CurrencyId = currencyId,
                BetCount = Convert.ToInt32(reader["BetCount"]),
                Turnover = Convert.ToDecimal(reader["TurnoverBase" + baseColumnSuffix]),
                CustomerWinLoss = Convert.ToDecimal(reader["WinlostBase" + baseColumnSuffix]),
                SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWLBase" + baseColumnSuffix]),
                SubsidiaryComm = Convert.ToDecimal(reader["SubsidiaryCommBase" + baseColumnSuffix])
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
            total.SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWLBase"]);
            total.SubsidiaryComm += Convert.ToDecimal(reader["SubsidiaryCommBase"]);
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
            total.SubsidiaryWinLoss = detail.SubsidiaryWinLoss;
            total.SubsidiaryComm += detail.SubsidiaryComm;
        }

        private SubsidiaryPerformanceDetail CalculateDetailGrandTotal(IEnumerable<SubsidiaryPerformanceDetail> total)
        {
            return new SubsidiaryPerformanceDetail
            {
                BetCount = total.Sum(x => x.BetCount),
                Turnover = total.Sum(x => x.Turnover),
                CustomerWinLoss = total.Sum(x => x.CustomerWinLoss),
                SubsidiaryWinLoss = total.Sum(x => x.SubsidiaryWinLoss),
                SubsidiaryComm = total.Sum(x => x.SubsidiaryComm)
            };
        }

        private void AssignParameter(IObjectDb objectDb, SubsidiaryPerformanceMetaModel metaModel)
        {
            objectDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            objectDb.AddInParameter("MoveBetsDay", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            objectDb.AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            objectDb.AddInParameter("IncludeRaw", DbType.Boolean, metaModel.IncludeRaw);
            objectDb.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            objectDb.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
        }
    }
}