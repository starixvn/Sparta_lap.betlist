// -----------------------------------------------------------------------
// <copyright file="ISportTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Common
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
        IDictionary<string, int> GetAllSports();

        IList<Sport> GetAllWHSports();

        DataTable GetSportsAndProducts(int licenseeId, string languageCode);
    }
}