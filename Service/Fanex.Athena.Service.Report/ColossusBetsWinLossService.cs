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
        private readonly IColossusBetsWinLossDataAccess _colossusBetsWinLossDataAccess;

        public ColossusBetsWinLossService()
        {
            this._colossusBetsWinLossDataAccess = new ColossusBetsWinLossDataAccess();
        }

        public SortableList<ThirdPartyWinLoss> GetAgentWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            return metaModel.IsValid
                       ? this._colossusBetsWinLossDataAccess.GetAgentWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<ThirdPartyWinLoss>();
        }

        public SortableList<ThirdPartyWinLoss> GetCustomerWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            return metaModel.IsValid
                       ? this._colossusBetsWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                       : new SortableList<ThirdPartyWinLoss>();
        }

        public SortableList<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            var result = this._colossusBetsWinLossDataAccess.GetSummaryWinloss(metaModel, out summaryTotal);
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

        public IList<ThirdPartyWinLoss> TotalCalc(IList<ThirdPartyWinLoss> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new SortableList<ThirdPartyWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new ThirdPartyWinLoss
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