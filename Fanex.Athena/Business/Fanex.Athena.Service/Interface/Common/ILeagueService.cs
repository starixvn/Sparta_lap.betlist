// -----------------------------------------------------------------------
// <copyright file="ILeagueService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILeagueService
    {
        IList<League> GetLeaguesBySport(LeagueMetaModel metaModel);
    }
}