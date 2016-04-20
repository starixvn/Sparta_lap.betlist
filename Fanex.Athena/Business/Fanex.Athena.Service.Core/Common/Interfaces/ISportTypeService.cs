// -----------------------------------------------------------------------
// <copyright file="ISportTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    #region

    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ISportTypeService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        IDictionary<int, string> GetAllSports();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<Sport> GetAllWHSports();

        IEnumerable<SportProductItem> GetSportsAndProducts(int licenseeId, string languageCode, int? productId = null);
    }
}