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
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    /// <summary>
    ///     The licensee win loss service.
    /// </summary>
    public class WinLossByLeagueService : IWinLossByLeagueService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _balance service.
        /// </summary>
        private readonly IBalanceUpService _balanceService;

        /// <summary>
        ///     The _licensee win loss data access.
        /// </summary>
        private readonly IWinLossByLeagueDataAccess _winLossByLeagueDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinLossByLeagueService" /> class.
        /// </summary>
        public WinLossByLeagueService()
        {
            this._balanceService = new BalanceUpService();
            this._winLossByLeagueDataAccess = new WinLossByLeagueDataAccess(this._balanceService.BalanceUpInfo);
        }

        #endregion Constructors and Destructors

        #region IWinLossByLeagueService Members

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

        #endregion IWinLossByProductService Members
    }
}