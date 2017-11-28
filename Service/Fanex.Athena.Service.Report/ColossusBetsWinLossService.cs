// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossService.cs" company="">
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.ResultModel;

    public class ColossusBetsWinLossService : IColossusBetsWinLossService
    {
        private readonly IColossusBetsWinLossDataAccess _colossusBetsWinLossDataAccess;

        public ColossusBetsWinLossService()
        {
            this._colossusBetsWinLossDataAccess = new ColossusBetsWinLossDataAccess();
        }

        public SortableReportResultModel<ColossusBetWinLoss> GetWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return _colossusBetsWinLossDataAccess.GetWinlossDetail(metaModel);
        }

        public SortableReportResultModel<ColossusBetWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel)
        {
            var result = this._colossusBetsWinLossDataAccess.GetSummaryWinloss(metaModel);
            var date = new System.DateTime();
            foreach (var item in result.DataList)
            {
                if (date != item.Date)
                {
                    date = item.Date;
                    item.RowSpan = result.DataList.Count(x => x.Date == item.Date);
                }
                else
                {
                    item.RowSpan = -1;
                }
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