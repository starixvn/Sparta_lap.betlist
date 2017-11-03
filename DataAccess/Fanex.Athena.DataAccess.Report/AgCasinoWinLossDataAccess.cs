// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Interfaces;
    using Model.Core.ResultModel;

    public class AgCasinoWinLossDataAccess : ThirdPartyWinLossDataAccess, IAgCasinoWinLossDataAccess
    {
        public SortableReportResultModel<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel)
        {
            return GetSummaryWinloss(metaModel, "Lrf_Rpt_AGCasinoWinloss");
        }

        public SortableReportResultModel<ThirdPartyWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return GetCustomerWinlossDetail(metaModel, "LRF_Rpt_AGCasinoWinloss_Mem");
        }

        public SortableReportResultModel<ThirdPartyWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            return GetAgentWinlossDetail(metaModel, "LRF_Rpt_AGCasinoWinloss_Ag");
        }
    }
}