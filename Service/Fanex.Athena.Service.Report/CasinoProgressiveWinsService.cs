// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsService.cs" company="">
//
// </copyright>
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

    public class CasinoProgressiveWinsService : ICasinoProgressiveWinsService
    {
        #region Constants and Fields

        private readonly IBalanceUpService _balanceService;

        private readonly ICasinoProgressiveWinsDataAccess _casinoProgressiveWinsDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        public CasinoProgressiveWinsService()
        {
            this._balanceService = new BalanceUpService();
            this._casinoProgressiveWinsDataAccess = new CasinoProgressiveWinsDataAccess(this._balanceService.BalanceUpInfo);
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        public IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? this._casinoProgressiveWinsDataAccess.GetDataList(metaModel)
                       : new List<CasinoProgressiveWins>();
        }

        #endregion Public Methods and Operators
    }
}