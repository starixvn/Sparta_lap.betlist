namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Extensions;
    using Core;
    using Core.Configuration.Interfaces;
    using Core.Exception;
    using Data;
    using Extension.Utility;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report;
    using Model.Report.MetaModel;

    public class PromotionPrizeDataAccess : BaseDataAccess, IPromotionPrizeDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public PromotionPrizeDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ReportResultModel<PromotionPrize> GetData(PromotionPrizeMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            var uspName = "Lrf_Rpt_PP_PromotionPrize";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    db.AddInParameter("AgentId", DbType.Int32, metaModel.AgentId);
                    db.AddInParameter("UserName", DbType.String, metaModel.UserName);
                    db.AddInParameter("FromDate", DbType.Date, metaModel.FromDate);
                    db.AddInParameter("ToDate", DbType.Date, metaModel.ToDate);
                    db.AddInParameter("MoveBetsDay", DbType.Date, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, currencyList);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, uspName, metaModel);
            }
        }

        private static IDictionary<string, PromotionPrize> InitSummaryTotal()
        {
            var summaryTotal = new Dictionary<string, PromotionPrize>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(
                    currency,
                    new PromotionPrize
                    {
                        Amount = 0,
                        CurrencyName = currency
                    });
            }

            return summaryTotal;
        }

        private ReportResultModel<PromotionPrize> GetData(IDataReader reader, IDictionary<int, string> currencyList)
        {
            var dataList = new List<PromotionPrize>();
            var summaryTotal = InitSummaryTotal();

            while (reader.Read())
            {
                var currencyId = reader.ToInt("CurrencyId");
                var item = new PromotionPrize
                {
                    SystemId = reader.ToString("UserName"),
                    ExternalId = reader.ToString("NickName"),
                    CurrencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),
                    Amount = reader.ToDecimal("Amount"),
                    Date = reader.ToDateTime("Date")
                };

                dataList.Add(item);
            }

            reader.NextResult();
            if (reader.Read())
            {
                foreach (var total in summaryTotal)
                {
                    total.Value.Amount = reader.ToDecimal("SumAmount" + total.Key);
                }
            }

            return new ReportResultModel<PromotionPrize>
            {
                DataList = dataList,
                SummaryTotal = summaryTotal,
                TotalRows = dataList.Count
            };
        }
    }
}