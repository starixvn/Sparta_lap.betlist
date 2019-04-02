// -----------------------------------------------------------------------
// <copyright file="ISettledMatchCountDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;

    /// <summary>
    ///     Settle Match Count Interface.
    /// </summary>
    public interface ISettledMatchCountDataAccess
    {
        Task<IList<SettledMatchCount>> GetDataList(bool isShowAll, int licenseeId);
    }
}