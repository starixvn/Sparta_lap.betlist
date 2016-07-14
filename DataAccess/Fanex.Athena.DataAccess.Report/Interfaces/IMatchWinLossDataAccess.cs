// -----------------------------------------------------------------------
// <copyright file="IMatchWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public interface IMatchWinLossDataAccess
    {
        IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel);

        DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel);

        IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel);

        IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel);
    }
}