// -----------------------------------------------------------------------
// <copyright file="ILicenseeWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ILicenseeWinLossDataAccess
    {
        SortableSummaryReportResultModel<LicenseeWinLoss> GetAgentWinlossDetail(LicenseeWinLossMetaModel metaModel);

        SortableSummaryReportResultModel<LicenseeWinLoss> GetCustomerWinlossDetail(LicenseeWinLossMetaModel metaModel);

        SortableSummaryReportResultModel<LicenseeWinLoss> GetSummaryWinloss(LicenseeWinLossMetaModel metaModel);
    }
}