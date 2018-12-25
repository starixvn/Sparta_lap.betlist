// -----------------------------------------------------------------------
// <copyright file="IMicroGamingWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using System.Collections.Generic;

    public interface IMicroGamingWinLossDataAccess
    {
        ReportResultModel<MicroGamingWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}