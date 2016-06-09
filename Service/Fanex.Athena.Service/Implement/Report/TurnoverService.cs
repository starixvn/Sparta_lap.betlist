// -----------------------------------------------------------------------
// <copyright file="TurnoverService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// Turnover service class.
    /// </summary>
    public class TurnoverService : ITurnoverService
    {
        private readonly ITurnoverDataAccess _turnoverDataAccess;

        public TurnoverService()
        {
            _turnoverDataAccess = new TurnoverDataAccess();
        }

        /// <summary>
        /// Get turnover data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The data list.</returns>
        public IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _turnoverDataAccess.GetDataList(metaModel)
                       : new List<TurnoverDTO>();
        }

        /// <summary>
        /// Calculate summary data.
        /// </summary>
        /// <param name="dataList">The original data.</param>
        /// <returns>The summary data.</returns>
        public TurnoverDTO SumTotalData(IList<TurnoverDTO> dataList)
        {
            var resultTotal = new TurnoverDTO();
            resultTotal.Deposit = dataList.Sum(p => p.Deposit);
            resultTotal.Withdrawal = dataList.Sum(p => p.Withdrawal);
            resultTotal.Bonus = dataList.Sum(p => p.Bonus);
            resultTotal.Adjustment = dataList.Sum(p => p.Adjustment);
            resultTotal.Turnover = dataList.Sum(p => p.Turnover);
            resultTotal.DrawGame = dataList.Sum(p => p.DrawGame);
            resultTotal.NetTurnover = dataList.Sum(p => p.NetTurnover);
            resultTotal.PendingFunds = dataList.Sum(p => p.PendingFunds);
            resultTotal.WinLoss = dataList.Sum(p => p.WinLoss);
            resultTotal.PlayerComm = dataList.Sum(p => p.PlayerComm);
            resultTotal.ClosingBalance = dataList.Sum(p => p.ClosingBalance);
            resultTotal.AffiliateTransfer = dataList.Sum(p => p.AffiliateTransfer);
            resultTotal.Transfer3rd = dataList.Sum(p => p.Transfer3rd);
            resultTotal.TurnoverBA = dataList.Sum(p => p.TurnoverBA);
            resultTotal.CommBA = dataList.Sum(p => p.CommBA);
            resultTotal.AmountDueToBA = dataList.Sum(p => p.AmountDueToBA);
            return resultTotal;
        }
    }
}