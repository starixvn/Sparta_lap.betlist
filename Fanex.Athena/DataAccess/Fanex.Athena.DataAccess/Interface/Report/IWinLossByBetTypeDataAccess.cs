// -----------------------------------------------------------------------
// <copyright file="IWinLossByBetTypeDataAccess.cs" company="Nexcel Solution">
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
    ///     Win Loss By Bet Type Data Access.
    /// </summary>
    public interface IWinLossByBetTypeDataAccess
    {
        IList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel);

        DataSet GetDataSetForExcel(WinLossByBetTypeMetaModel metaModel);
    }
}