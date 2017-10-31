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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.ResultModel;

    public class AgCasinoWinLossService : IAgCasinoWinLossService
    {
        private readonly IAgCasinoWinLossDataAccess _agcasinoWinLossDataAccess;

        public AgCasinoWinLossService()
        {
            _agcasinoWinLossDataAccess = new AgCasinoWinLossDataAccess();
        }

        public ReportResultModel<AgCasinoWinLoss> GetAgentWinlossDetail(AgCasinoWinLossMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _agcasinoWinLossDataAccess.GetAgentWinlossDetail(metaModel)
                       : new ReportResultModel<AgCasinoWinLoss>();
        }

        public ReportResultModel<AgCasinoWinLoss> GetCustomerWinlossDetail(AgCasinoWinLossMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _agcasinoWinLossDataAccess.GetCustomerWinlossDetail(metaModel)
                       : new ReportResultModel<AgCasinoWinLoss>();
        }

        public ReportResultModel<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel)
        {
            var result = _agcasinoWinLossDataAccess.GetSummaryWinloss(metaModel);
            var date = new DateTime();
            var index = 1;
            foreach (var item in result.DataList)
            {
                if (date != item.Date)
                {
                    date = item.Date ?? new DateTime();
                    item.RowSpan = result.DataList.Count(x => x.Date == item.Date);
                    item.Index = index++;
                }
                else
                {
                    item.RowSpan = -1;
                }
            }

            return result;
        }

        public IEnumerable<AgCasinoWinLoss> CalculateTotal(IEnumerable<AgCasinoWinLoss> dataList)
        {
            if (!dataList.Any())
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