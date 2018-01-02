namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IAgCasinoBalanceDataAccess
    {
        SortableReportResultModel<AgCasinoBalance> GetAgentBalanceDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<AgCasinoBalance> GetCustomerBalanceDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<AgCasinoBalance> GetSummaryBalance(ThirdPartyWinLossMetaModel metaModel);
    }
}