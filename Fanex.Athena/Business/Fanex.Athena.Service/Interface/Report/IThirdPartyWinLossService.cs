// -----------------------------------------------------------------------
// <copyright file="IThirdPartyWinLossService.cs" company="Microsoft">
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
    public interface IThirdPartyWinLossService
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
        SortableList<ThirdPartyWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal);

        /// <summary>
        /// The get customer win-oss detail table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="summaryTotal">The summary data.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<ThirdPartyWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal);

        /// <summary>
        /// The get summary win-loss table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="summaryTotal">The summary total.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal);

        /// <summary>
        /// Calculate summary data.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The summary data.</returns>
        IList<ThirdPartyWinLoss> TotalCalc(IList<ThirdPartyWinLoss> dataList);

        #endregion Public Methods and Operators
    }
}
