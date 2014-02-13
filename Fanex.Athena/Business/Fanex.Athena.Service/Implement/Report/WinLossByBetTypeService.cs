// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class WinLossByBetTypeService : IWinLossByBetTypeServive
    {
        #region Field and constants

        /// <summary>
        ///     The _balance up service.
        /// </summary>
        private readonly IBalanceUpService _balanceUpService;

        /// <summary>
        ///     Win Loss By Bet Type Data Access.
        /// </summary>
        private readonly WinLossByBetTypeDataAccess _winLossByBetTypeDataAccess;

        #endregion Field and constants

        #region Constructor

        public WinLossByBetTypeService()
        {
            this._balanceUpService = new BalanceUpService();
            this._winLossByBetTypeDataAccess = new WinLossByBetTypeDataAccess(this._balanceUpService.BalanceUpInfo);
        }

        #endregion Constructor

        public SortableList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _winLossByBetTypeDataAccess.GetDataList(metaModel)
                       : new SortableList<WinLossByBetType>();
        }
    }
}