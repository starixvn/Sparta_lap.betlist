// -----------------------------------------------------------------------
// <copyright file="IRacingCommissionService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<RacingCommission> GetDataList(RacingCommissionMetaModel metaModel, ref RacingCommission total);

        #endregion Public Methods and Operators
    }
}