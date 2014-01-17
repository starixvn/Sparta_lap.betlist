// -----------------------------------------------------------------------
// <copyright file="ILeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ILeagueDataAccess
    {
        IList<League> GetLeaguesBySport(LeagueMetaModel metaModel);  
    }
}