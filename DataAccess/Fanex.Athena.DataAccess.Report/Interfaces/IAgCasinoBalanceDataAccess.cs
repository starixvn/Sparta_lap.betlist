namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IAgCasinoBalanceDataAccess
    {
        SortableSummaryReportResultModel<AgCasinoBalance> GetAgentBalanceDetail(AgCasinoBalanceMetaModel metaModel);

        SortableSummaryReportResultModel<AgCasinoBalance> GetCustomerBalanceDetail(AgCasinoBalanceMetaModel metaModel);

        SortableSummaryReportResultModel<AgCasinoBalance> GetSummaryBalance(AgCasinoBalanceMetaModel metaModel);
    }
}