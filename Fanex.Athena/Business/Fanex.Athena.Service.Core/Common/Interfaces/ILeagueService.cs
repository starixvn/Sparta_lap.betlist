// -----------------------------------------------------------------------
// <copyright file="ILeagueService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILeagueService
    {
        IList<League> GetLeaguesBySport(LeagueMetaModel metaModel);
    }
}