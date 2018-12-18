// -----------------------------------------------------------------------
// <copyright file="IThirdPartyWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IThirdPartyWinLossDataAccess<T>
    {
        ReportResultModel<T> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel, string spname);

        ReportResultModel<T> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel, string spname);

        ReportResultModel<T> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, string spname);
    }
}