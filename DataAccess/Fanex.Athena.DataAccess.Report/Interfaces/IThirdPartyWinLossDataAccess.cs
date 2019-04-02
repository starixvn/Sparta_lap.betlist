// -----------------------------------------------------------------------
// <copyright file="IThirdPartyWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IThirdPartyWinLossDataAccess<T>
    {
        Task<ReportResultModel<T>> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList, string spname);

        Task<ReportResultModel<T>> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList, string spname);

        Task<ReportResultModel<T>> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList, string spname);
    }
}