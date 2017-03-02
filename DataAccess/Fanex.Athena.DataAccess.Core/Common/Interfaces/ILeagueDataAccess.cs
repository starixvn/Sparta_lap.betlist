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

    public interface ILeagueDataAccess
    {
        IEnumerable<League> GetLeaguesBySport(LeagueMetaModel metaModel);
    }
}