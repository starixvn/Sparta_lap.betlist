// -----------------------------------------------------------------------
// <copyright file="ILeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ILeagueDataAccess
    {
        IList<League> GetLeaguesBySport(LeagueMetaModel metaModel);  
    }
}