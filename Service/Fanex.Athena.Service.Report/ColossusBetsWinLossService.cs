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
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    public class ColossusBetsWinLossService : IColossusBetsWinLossService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly IColossusBetsWinLossDataAccess _colossusBetsWinLossDataAccess;

        public ColossusBetsWinLossService()
        {
            _balanceUpService = new BalanceUpService();
            this._colossusBetsWinLossDataAccess = new ColossusBetsWinLossDataAccess(_balanceUpService.BalanceUpInfo);
        }
        
        public SortableList<ColossusBetWinLoss> GetWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ColossusBetWinLoss>();
            return metaModel.IsValid
                       ? this._colossusBetsWinLossDataAccess.GetWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<ColossusBetWinLoss>();
        }

        public SortableList<ColossusBetWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ColossusBetWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ColossusBetWinLoss>();
            var result = this._colossusBetsWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
            var date = new System.DateTime();
            foreach (var item in result)
            {
                if (date != item.Date)
                {
                    date = item.Date;
                    item.RowSpan = result.Count(x => x.Date == item.Date);
                }
                else
                {
                    item.RowSpan = -1;
                }
            }

            return result;
        }

        public IList<ColossusBetWinLoss> CalculateTotal(IList<ColossusBetWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new List<ColossusBetWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new ColossusBetWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                WinningPrize = g.Sum(p => p.WinningPrize),
                CashOut = g.Sum(p => p.CashOut),
                CashIn = g.Sum(p => p.CashIn),
                CommissionShare = g.Sum(p => p.CommissionShare),
                GrandPrize = g.Sum(p => p.GrandPrize)
            }).ToList();
        }
    }
}