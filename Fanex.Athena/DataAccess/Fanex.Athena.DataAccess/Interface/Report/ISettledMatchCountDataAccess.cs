// -----------------------------------------------------------------------
// <copyright file="ISettledMatchCountDataAccess.cs" company="Nexcel Solution">
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
    using Fanex.Athena.Models.DTO.Report;

    /// <summary>
    ///     Settle Match Count Interface.
    /// </summary>
    public interface ISettledMatchCountDataAccess
    {
        IList<SettledMatchCount> GetDataList(bool isShowAll, int licenseeId);
    }
}