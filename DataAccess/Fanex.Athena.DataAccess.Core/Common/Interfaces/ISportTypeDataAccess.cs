// -----------------------------------------------------------------------
// <copyright file="ISportTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISportTypeDataAccess
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