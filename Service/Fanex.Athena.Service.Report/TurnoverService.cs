// -----------------------------------------------------------------------
// <copyright file="TurnoverService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    public class TurnoverService : ITurnoverService
    {
        private readonly ITurnoverDataAccess _turnoverDataAccess;

        public TurnoverService()
        {
            _turnoverDataAccess = new TurnoverDataAccess();
        }

        public TurnoverService(ITurnoverDataAccess dataAccess)
        {
            _turnoverDataAccess = dataAccess;
        }

        public IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _turnoverDataAccess.GetDataList(metaModel)
                       : new List<TurnoverDTO>();
        }

        public TurnoverDTO SumTotalData(IList<TurnoverDTO> dataList)
        {
            var resultTotal = new TurnoverDTO
            {
                Deposit = dataList.Sum(p => p.Deposit),
                Withdrawal = dataList.Sum(p => p.Withdrawal),
                Bonus = dataList.Sum(p => p.Bonus),
                Adjustment = dataList.Sum(p => p.Adjustment),
                Turnover = dataList.Sum(p => p.Turnover),
                DrawGame = dataList.Sum(p => p.DrawGame),
                NetTurnover = dataList.Sum(p => p.NetTurnover),
                PendingFunds = dataList.Sum(p => p.PendingFunds),
                WinLoss = dataList.Sum(p => p.WinLoss),
                PlayerComm = dataList.Sum(p => p.PlayerComm),
                ClosingBalance = dataList.Sum(p => p.ClosingBalance),
                AffiliateTransfer = dataList.Sum(p => p.AffiliateTransfer),
                Transfer3rd = dataList.Sum(p => p.Transfer3rd),
                TurnoverBA = dataList.Sum(p => p.TurnoverBA),
                CommBA = dataList.Sum(p => p.CommBA),
                AmountDueToBA = dataList.Sum(p => p.AmountDueToBA)
            };
            return resultTotal;
        }
    }
}