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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<WinLossByLeague> GetWinlossByLeagueCustomers(WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalrows);
    }
}