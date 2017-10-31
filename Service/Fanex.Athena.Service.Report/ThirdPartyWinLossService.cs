// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThirdPartyWinLossService.cs" company="">
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

    public class ThirdPartyWinLossService : IThirdPartyWinLossService
    {
        private readonly IThirdPartyWinLossDataAccess _thirdPartyWinLossDataAccess;

        public ThirdPartyWinLossService()
        {
            this._thirdPartyWinLossDataAccess = new ThirdPartyWinLossDataAccess();
        }

        public SortableReportResultModel<ThirdPartyWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return _thirdPartyWinLossDataAccess.GetAgentWinlossDetail(metaModel);
        }

        public SortableReportResultModel<ThirdPartyWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return _thirdPartyWinLossDataAccess.GetCustomerWinlossDetail(metaModel);
        }

        public SortableReportResultModel<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel)
        {
            var result = this._thirdPartyWinLossDataAccess.GetSummaryWinloss(metaModel);
            var date = new System.DateTime();
            var index = 1;
            var dataList = result.DataList;
            foreach (var item in dataList)
            {
                if (date != item.Date)
                {
                    date = item.Date ?? new System.DateTime();
                    item.RowSpan = dataList.Count(x => x.Date == item.Date);
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