// -----------------------------------------------------------------------
// <copyright file="IRacingCommissionService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IRacingCommissionService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The get data list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="total">The summary data.</param>
        /// <returns>The racing commission data list.</returns>
        IList<RacingCommission> GetDataList(RacingCommissionMetaModel metaModel, ref RacingCommission total);

        #endregion Public Methods and Operators
    }
}