// -----------------------------------------------------------------------
// <copyright file="ILeagueService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ILeagueService
    {
        IEnumerable<League> GetLeaguesBySport(LeagueMetaModel metaModel);
    }
}