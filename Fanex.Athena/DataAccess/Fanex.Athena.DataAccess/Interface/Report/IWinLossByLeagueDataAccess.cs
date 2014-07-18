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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<WinLossByLeague> GetWinlossByLeagueCustomers(
            WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalRows);
    }
}