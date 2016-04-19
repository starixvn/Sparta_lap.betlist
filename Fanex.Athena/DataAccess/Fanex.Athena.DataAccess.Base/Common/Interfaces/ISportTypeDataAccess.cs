// -----------------------------------------------------------------------
// <copyright file="ISportTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;

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