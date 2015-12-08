// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinLossByLeagueService.cs" company="Nexcel">
// The  win loss by product service.
// </copyright>
// <summary>
//   The  win loss by product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

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