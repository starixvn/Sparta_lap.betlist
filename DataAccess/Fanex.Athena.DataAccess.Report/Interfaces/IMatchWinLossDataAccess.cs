// -----------------------------------------------------------------------
// <copyright file="IMatchWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IMatchWinLossDataAccess
    {
        IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel);

        DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel);

        IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel);

        IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel);
    }
}