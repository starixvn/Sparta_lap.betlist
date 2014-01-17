// -----------------------------------------------------------------------
// <copyright file="IWinLossByLeagueService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface IWinLossByLeagueService
    {
        IList<WinLossByLeague> GetWinlossByLeagues(WinLossByLeagueMetaModel metaModel);

        IList<WinLossByLeague> GetWinlossByLeagueCustomers(WinLossByLeagueMetaModel metaModel, ref int totalrows);
    }
}