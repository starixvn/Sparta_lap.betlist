// -----------------------------------------------------------------------
// <copyright file="ISettledMatchCountDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
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