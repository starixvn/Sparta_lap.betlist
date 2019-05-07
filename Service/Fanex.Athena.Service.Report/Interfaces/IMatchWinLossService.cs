// -----------------------------------------------------------------------
// <copyright file="IMatchWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IMatchWinLossService
    {
        Task<IList<MatchWinLoss>> GetDataList(MatchWinLossMetaModel metaModel);

        Task<DataSet> GetDetailDataSet(MatchWinLossMetaModel metaModel);

        Task<IList<MatchWinLoss>> GetDetailDataList(MatchWinLossMetaModel metaModel);

        Task<IList<MatchWinLoss>> GetOutrightParlayList(MatchWinLossMetaModel metaModel);
    }
}