// -----------------------------------------------------------------------
// <copyright file="IMatchWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IMatchWinLossService
    {
        /// <summary>
        ///     The get data list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns> The match win-loss. </returns>
        IList<MatchWinLoss> GetDataList(MatchWinLossMetaModel metaModel);

        /// <summary>
        ///     Get data for export excel only.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The data set.</returns>
        DataSet GetDetailDataSet(MatchWinLossMetaModel metaModel);

        /// <summary>
        ///     The get detail data list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns> The match win-loss list.</returns>
        IList<MatchWinLoss> GetDetailDataList(MatchWinLossMetaModel metaModel);

        /// <summary>
        ///     The get data list outright mix-parlay.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns> The match win-loss list.</returns>
        IList<MatchWinLoss> GetOutrightParlayList(MatchWinLossMetaModel metaModel);

        /// <summary>
        ///     The get data list outright mix-parlay.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns> The data set. </returns>
        DataSet GetOutrightParlayDataSet(MatchWinLossMetaModel metaModel);
    }
}