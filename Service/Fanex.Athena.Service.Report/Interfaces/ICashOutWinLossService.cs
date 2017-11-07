// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICashOutWinLossService.cs" company="">
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

    public interface ICashOutWinLossService
    {
        SortableReportResultModel<CashOutWinLoss> GetDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<CashOutWinLoss> GetSummary(ThirdPartyWinLossMetaModel metaModel);

        IList<CashOutWinLoss> TotalCalc(IList<CashOutWinLoss> dataList);
    }
}