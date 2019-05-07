// -----------------------------------------------------------------------
// <copyright file="IMatchWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IMatchWinLossDataAccess
    {
        Task<IList<MatchWinLoss>> GetDataList(MatchWinLossMetaModel metaModel);

        Task<DataSet> GetDetailDataSet(MatchWinLossMetaModel metaModel);

        Task<IList<MatchWinLoss>> GetDetailDataList(MatchWinLossMetaModel metaModel);

        Task<IList<MatchWinLoss>> GetOutrightParlayList(MatchWinLossMetaModel metaModel);
    }
}