// -----------------------------------------------------------------------
// <copyright file="IMicroGamingWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// The third party licensee win loss service.
    /// </summary>
    public interface IMicroGamingWinLossService
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get summary win-loss table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel, out IDictionary<string, MicroGamingWinLoss> summaryTotal);

        /// <summary>
        /// Calculate summary data.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The summary data.</returns>
        IList<MicroGamingWinLoss> TotalCalc(IList<MicroGamingWinLoss> dataList);

        #endregion Public Methods and Operators
    }
}
