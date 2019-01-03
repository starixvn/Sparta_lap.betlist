// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossService.cs" company="">
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.ResultModel;

    public class ColossusBetsWinLossService : IColossusBetsWinLossService
    {
        private readonly IColossusBetsWinLossDataAccess _colossusBetsWinLossDataAccess;

        public ColossusBetsWinLossService(IColossusBetsWinLossDataAccess dataAccess)
        {
            this._colossusBetsWinLossDataAccess = dataAccess;
        }

        public SortableReportResultModel<ColossusBetWinLoss> GetWinlossDetail(
            ColossusBetsWinLossMetaModel metaModel, 
            IDictionary<int, string> currencyList)
        {
            return _colossusBetsWinLossDataAccess.GetWinlossDetail(metaModel, currencyList);
        }

        public SortableReportResultModel<ColossusBetWinLoss> GetSummaryWinloss(
             ColossusBetsWinLossMetaModel metaModel,
             IDictionary<int, string> currencyList)
        {
            var result = this._colossusBetsWinLossDataAccess.GetSummaryWinloss(metaModel, currencyList);
            var date = new System.DateTime();
            var index = 0;
            foreach (var item in result.DataList)
            {
                if (date != item.Date)
                {
                    date = item.Date;
                    item.RowSpan = result.DataList.Count(x => x.Date == item.Date);
                    index++;
                }
                else
                {
                    item.RowSpan = -1;
                }

                item.Index = index;
            }

            return result;
        }

        public IEnumerable<ColossusBetWinLoss> CalculateTotal(IEnumerable<ColossusBetWinLoss> dataList)
        {
            if (!dataList.Any())
            {
                return new List<ColossusBetWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new ColossusBetWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                MemberWinning = g.Sum(p => p.MemberWinning),
                CashOut = g.Sum(p => p.CashOut),
                CashOutShare = g.Sum(p => p.CashOutShare),
                CommissionShare = g.Sum(p => p.CommissionShare),
                GrandPrize = g.Sum(p => p.GrandPrize)
            }).ToList();
        }
    }
}