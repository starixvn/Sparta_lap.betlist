// -----------------------------------------------------------------------
// <copyright file="IWinLossByLeagueDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface IWinLossByLeagueDataAccess
    {
        IList<WinLossByLeague> GetWinlossByLeagues(
            WinLossByLeagueMetaModel metaModel);

        IList<WinLossByLeague> GetWinlossByLeagueCustomers(
            WinLossByLeagueMetaModel metaModel, ref int totalrows);        
    }
}