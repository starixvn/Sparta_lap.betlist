namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IAgCasinoBalanceDataAccess
    {
        SortableReportResultModel<AgCasinoBalance> GetAgentBalanceDetail(AgCasinoBalanceMetaModel metaModel);

        SortableReportResultModel<AgCasinoBalance> GetCustomerBalanceDetail(AgCasinoBalanceMetaModel metaModel);

        SortableReportResultModel<AgCasinoBalance> GetSummaryBalance(AgCasinoBalanceMetaModel metaModel);
    }
}