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
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     Win Loss By Bet Type Data Access.
    /// </summary>
    public interface IWinLossByBetTypeDataAccess
    {
        SortableList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel);
    }
}