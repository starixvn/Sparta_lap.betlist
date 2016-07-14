// -----------------------------------------------------------------------
// <copyright file="ISettledMatchCountDataAccess.cs" company="">
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
    using Fanex.Athena.Model.Report.DTO;

    /// <summary>
    ///     Settle Match Count Interface.
    /// </summary>
    public interface ISettledMatchCountDataAccess
    {
        IList<SettledMatchCount> GetDataList(bool isShowAll, int licenseeId);
    }
}