// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinLossByLeagueService.cs" company="">
// The  win loss by product service.
// </copyright>
// <summary>
//   The  win loss by product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    public class WinLossByLeagueService : IWinLossByLeagueService
    {
        private readonly IWinLossByLeagueDataAccess _winLossByLeagueDataAccess;

        public WinLossByLeagueService()
        {
            this._winLossByLeagueDataAccess = new WinLossByLeagueDataAccess();
        }

        public IEnumerable<WinLossByLeague> GetWinlossByLeagues(
            WinLossByLeagueMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? this._winLossByLeagueDataAccess.GetWinlossByLeagues(metaModel)
                       : new List<WinLossByLeague>();
        }

        public IEnumerable<WinLossByLeague> GetWinlossByLeagueCustomers(
            WinLossByLeagueMetaModel metaModel, out WinLossByLeague grandTotal, ref int totalrows)
        {
            grandTotal = new WinLossByLeague();
            return metaModel.IsValid ?
                this._winLossByLeagueDataAccess.GetWinlossByLeagueCustomers(metaModel, out grandTotal, ref totalrows) :
                new List<WinLossByLeague>();
        }
    }
}