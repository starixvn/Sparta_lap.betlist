// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgCasinoBalanceService.cs" company="">
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

    public class AgCasinoBalanceService : IAgCasinoBalanceService
    {
        private readonly IBalanceUpService _balanceService;

        private readonly IAgCasinoBalanceDataAccess _licenseeBalanceDataAccess;

        public AgCasinoBalanceService()
        {
            this._balanceService = new BalanceUpService();
            this._licenseeBalanceDataAccess = new AgCasinoBalanceDataAccess(this._balanceService.BalanceUpInfo);
        }

        public SortableList<AgCasinoBalance> GetAgentBalanceDetail(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            return this._licenseeBalanceDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal);
        }

        public SortableList<AgCasinoBalance> GetCustomerBalanceDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal,
            out int totalRows)
        {
            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            return this._licenseeBalanceDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal, out totalRows);
        }

        public SortableList<AgCasinoBalance> GetSummaryBalance(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            var result = this._licenseeBalanceDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
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

        public IList<AgCasinoBalance> TotalCalc(IList<AgCasinoBalance> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<AgCasinoBalance>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new AgCasinoBalance
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                OpeningBalance = g.Sum(p => p.OpeningBalance),
                TransferIn = g.Sum(p => p.TransferIn),
                TransferOut = g.Sum(p => p.TransferOut),
                ClosingBalance = g.Sum(p => p.ClosingBalance),
            }).ToList();
        }
    }
}