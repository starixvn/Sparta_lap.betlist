// -----------------------------------------------------------------------
// <copyright file="ICustTurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
//  <change who="Alex Nguyen" date="2013.10.10">Add new method GetSportProduct and GetBettype </change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICustTurnoverDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode, out CustTurnover total, out int totalRow);
    }
}