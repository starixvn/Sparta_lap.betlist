// -----------------------------------------------------------------------
// <copyright file="IMatchWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public interface IMatchWinLossDataAccess
    {
        IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel);

        DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel);

        IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel);

        IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel);

        DataSet GetOutrightParlayDataSet(MatchWinLossMetaModel metaModel);
    }
}