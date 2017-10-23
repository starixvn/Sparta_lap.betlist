// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAgCasinoWinLossService.cs" company="">
// The i licensee win loss service.
// </copyright>
// <summary>
//   The i licensee win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IAgCasinoWinLossService
    {
        ReportResultModel<AgCasinoWinLoss> GetAgentWinlossDetail(AgCasinoWinLossMetaModel metaModel);

        ReportResultModel<AgCasinoWinLoss> GetCustomerWinlossDetail(AgCasinoWinLossMetaModel metaModel);

        ReportResultModel<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel);

        IEnumerable<AgCasinoWinLoss> CalculateTotal(IEnumerable<AgCasinoWinLoss> dataList);
    }
}