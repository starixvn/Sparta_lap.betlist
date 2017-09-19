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

    public class MicroGamingWinLossService : IMicroGamingWinLossService
    {
        private readonly IMicroGamingWinLossDataAccess _microGamingWinLossDataAccess;

        public MicroGamingWinLossService()
        {
            this._microGamingWinLossDataAccess = new MicroGamingWinLossDataAccess();
        }

        public SortableList<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel, out IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, MicroGamingWinLoss>();

            return metaModel.IsValid ?
                this._microGamingWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal) :
                new SortableList<MicroGamingWinLoss>();
        }

        public IList<MicroGamingWinLoss> TotalCalc(IList<MicroGamingWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<MicroGamingWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new MicroGamingWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                ProgressiveContribution = g.Sum(p => p.ProgressiveContribution),
                ProgressivePayout = g.Sum(p => p.ProgressivePayout),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                LicenseeNetWin = g.Sum(p => p.LicenseeNetWin),
                AmountDueToMGC = g.Sum(p => p.AmountDueToMGC),
                ProgressiveFee = g.Sum(p => p.ProgressiveFee)
            }).ToList();
        }
    }
}