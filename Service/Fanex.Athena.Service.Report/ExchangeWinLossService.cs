namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Model.Core.ResultModel;
    using Fanex.Athena.Model.Report.DTO;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Report.MetaModel;

    public class ExchangeWinLossService : IExchangeWinLossService
    {
        public ReportResultModel<ExchangeWinLoss> GetData(ExchangeWinLossMetaModel metaModel)
        {
            var dataList = GetMainData(metaModel);
            return new ReportResultModel<ExchangeWinLoss>
            {
                DataList = dataList,
                TotalDataList = GetTotalData(dataList),
                SummaryTotal = GetSummaryTotalData()
            };
        }

        private IEnumerable<ExchangeWinLoss> GetMainData(ExchangeWinLossMetaModel metaModel)
        {
            var data = new List<ExchangeWinLoss>();
            var dateCount = metaModel.ToDate.Subtract(metaModel.FromDate).Days;

            for (int i = 0; i < dateCount; i++)
            {
                data.Add(new ExchangeWinLoss
                {
                    Date = metaModel.FromDate.AddDays(i),
                    CurrencyName = i.ToString(),
                    CurrencyId = i,
                    ExternalId = "ExternalId" + i,
                    SystemId = "SystemId" + i,
                    Turnover = i,
                    TotalTransaction = i,
                    CustomerWL = i,
                    CustomerComm = i,
                    LicenseeWL = -i,
                    LicenseeComm = i,
                    AmountDueTo = i,
                    CusId = i,
                    NetTurnover = i
                });
            }

            return data;
        }

        private IEnumerable<ExchangeWinLoss> GetTotalData(IEnumerable<ExchangeWinLoss> mainData)
        {
            return mainData.GroupBy(x => new { x.CurrencyId, x.CurrencyName }).Select(g => new ExchangeWinLoss
            {
                CurrencyId = g.Key.CurrencyId,
                CurrencyName = g.Key.CurrencyName,
                Turnover = g.Sum(x => x.Turnover),
                TotalTransaction = g.Sum(x => x.TotalTransaction),
                CustomerWL = g.Sum(x => x.CustomerWL),
                CustomerComm = g.Sum(x => x.CustomerComm),
                LicenseeWL = g.Sum(x => x.LicenseeWL),
                LicenseeComm = g.Sum(x => x.LicenseeComm),
                AmountDueTo = g.Sum(x => x.AmountDueTo),
                NetTurnover = g.Sum(x => x.NetTurnover)
            }).ToList();
        }

        private IDictionary<string, ExchangeWinLoss> GetSummaryTotalData()
        {
            var summaryTotal = new Dictionary<string, ExchangeWinLoss>();

            foreach (var baseCurrency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(baseCurrency, new ExchangeWinLoss());
            }

            return summaryTotal;
        }
    }
}