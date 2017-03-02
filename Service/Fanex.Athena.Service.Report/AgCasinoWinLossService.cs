// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgCasinoWinLossService.cs" company="">
// The ag casino win loss service.
// </copyright>
// <summary>
//   The ag casino win loss service.
// </summary>
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

    public class AgCasinoWinLossService : IAgCasinoWinLossService
    {
        private readonly IBalanceUpService _balanceService;

        private readonly IAgCasinoWinLossDataAccess _agcasinoWinLossDataAccess;

        public AgCasinoWinLossService()
        {
            this._balanceService = new BalanceUpService();
            this._agcasinoWinLossDataAccess = new AgCasinoWinLossDataAccess(this._balanceService.BalanceUpInfo);
        }

        public SortableList<AgCasinoWinLoss> GetAgentWinlossDetail(
            AgCasinoWinLossMetaModel metaModel,
            out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            return metaModel.IsValid
                       ? this._agcasinoWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoWinLoss>();
        }

        public SortableList<AgCasinoWinLoss> GetCustomerWinlossDetail(
            AgCasinoWinLossMetaModel metaModel,
            out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            return metaModel.IsValid
                       ? this._agcasinoWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<AgCasinoWinLoss>();
        }

        public SortableList<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel, out IDictionary<string, AgCasinoWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            var result = this._agcasinoWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
            var date = new System.DateTime();
            var index = 1;
            foreach (var item in result)
            {
                if (date != item.Date)
                {
                    date = item.Date ?? new System.DateTime();
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

        public IList<AgCasinoWinLoss> TotalCalc(IList<AgCasinoWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<AgCasinoWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new AgCasinoWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL)
            }).ToList();
        }
    }
}