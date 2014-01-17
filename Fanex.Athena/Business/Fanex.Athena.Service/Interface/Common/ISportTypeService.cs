// -----------------------------------------------------------------------
// <copyright file="ISportTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    #region

    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ISportTypeService
    {
        IDictionary<string, int> GetAllSports();

        /// <summary>
        ///     The get sport by date.
        /// </summary>
        /// <param name="fromDate">
        ///     The from date.
        /// </param>
        /// <param name="toDate">
        ///     The to date.
        /// </param>
        /// <returns>
        /// The sport list.
        /// </returns>
        IList<Sport> GetSportByDate(DateTime fromDate, DateTime toDate);

        IList<SportProductItem> GetSportsAndProducts(int licenseeId, string languageCode, int? productId = null);
    }
}