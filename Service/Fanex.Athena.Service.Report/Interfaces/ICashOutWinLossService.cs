// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICashOutWinLossService.cs" company="">
// The i licensee win loss service.
// </copyright>
// <summary>
//   The i licensee win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICashOutWinLossService
    {
        #region Public Methods and Operators

        /// <summary>
        ///  The get agent win-loss detail table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="summaryTotal">The summary data.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetAgentWinlossDetail(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        /// <summary>
        /// The get customer win-oss detail table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="summaryTotal">The summary data.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetCustomerWinlossDetail(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        /// <summary>
        /// The get summary win-loss table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CashOutWinLoss> GetSummaryWinloss(CashOutWinLossMetaModel metaModel, out IDictionary<string, CashOutWinLoss> summaryTotal);

        /// <summary>
        /// Calculate summary data.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The summary data.</returns>
        IList<CashOutWinLoss> TotalCalc(IList<CashOutWinLoss> dataList);

        #endregion Public Methods and Operators
    }
}