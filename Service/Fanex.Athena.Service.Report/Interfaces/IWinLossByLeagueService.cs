// -----------------------------------------------------------------------
// <copyright file="IWinLossByLeagueService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IWinLossByLeagueService
    {
        IEnumerable<WinLossByLeague> GetWinlossByLeagues(WinLossByLeagueMetaModel metaModel);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WinLossByLeague> GetWinlossByLeagueCustomers(WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalrows);
    }
}