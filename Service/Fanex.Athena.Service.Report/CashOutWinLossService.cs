// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashOutWinLossService.cs" company="">
// The cash out win loss service.
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

    public class CashOutWinLossService : ICashOutWinLossService
    {
        private readonly IBalanceUpService _balanceService;

        private readonly ICashOutWinLossDataAccess _dataAccess;

        public CashOutWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._dataAccess = new CashOutWinLossDataAccess(this._balanceService.BalanceUpInfo);
        }

        public CashOutWinLossService(IBalanceUpService balanceService, ICashOutWinLossDataAccess dataAccess)
        {
            _balanceService = balanceService;
            _dataAccess = dataAccess;
        }

        public SortableList<CashOutWinLoss> GetDetail(
            CashOutWinLossMetaModel metaModel,
            out IDictionary<string, CashOutWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, CashOutWinLoss>();
            return _dataAccess.GetDetail(metaModel, out summaryTotal);
        }

        public SortableList<CashOutWinLoss> GetSummary(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, CashOutWinLoss>();
            var result = this._dataAccess.GetSummary(metaModel, out summaryTotal);
            var date = new System.DateTime();
            var index = 1;
            foreach (var item in result)
            {
                if (date != item.Date)
                {
                    date = item.Date;
                    item.RowSpan = result.Count(x => x.Date == item.Date);
                    item.Index = index++;
                }
                else
                {
                    item.RowSpan = -1;
                }
            }

            return result;
        }

        public IList<CashOutWinLoss> TotalCalc(IList<CashOutWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<CashOutWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new CashOutWinLoss
            {
                CurrencyName = g.Key,
                CashOutTimes = g.Sum(p => p.CashOutTimes),
                CashOutTickets = g.Sum(p => p.CashOutTickets),
                CashOutValue = g.Sum(p => p.CashOutValue),
                BuyBackAmountSB = g.Sum(p => p.BuyBackAmountSB),
                BuyBackAmountBA = g.Sum(p => p.BuyBackAmountBA),
                TicketPayout = g.Sum(p => p.TicketPayout),
                CashOutCommission = g.Sum(p => p.CashOutCommission),
                SettleAmountTicketPayout = g.Sum(p => p.SettleAmountTicketPayout),
                SettleAmountBuyBackComm = g.Sum(p => p.SettleAmountBuyBackComm),
                SettleAmountTotal = g.Sum(p => p.SettleAmountTotal),
            }).ToList();
        }
    }
}