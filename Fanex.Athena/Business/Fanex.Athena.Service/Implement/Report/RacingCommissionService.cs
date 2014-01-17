// -----------------------------------------------------------------------
// <copyright file="RacingCommissionService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class RacingCommissionService : IRacingCommissionService
    {
        #region Fields

        private readonly IBalanceUpService _balanceUpService;

        private readonly IRacingCommissionDataAccess _racingCommissionDataAccess;

        #endregion Fields

        #region Constructor

        public RacingCommissionService()
        {
            this._balanceUpService = new BalanceUpService();
            _racingCommissionDataAccess = new RacingCommissionDataAccess(this._balanceUpService.BalanceUpInfo);
        }

        #endregion Constructor

        public IList<RacingCommission> GetDataList(RacingCommissionMetaModel metaModel, ref RacingCommission total)
        {
            return metaModel.IsValid
                       ? _racingCommissionDataAccess.GetDataList(metaModel, ref total)
                       : new List<RacingCommission>();
        }
    }
}