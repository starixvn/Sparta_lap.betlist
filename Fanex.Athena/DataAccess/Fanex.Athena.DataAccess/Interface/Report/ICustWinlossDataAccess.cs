// -----------------------------------------------------------------------
// <copyright file="ICustWinlossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICustWinlossDataAccess
    {
        IList<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel, ref CustomerWinLoss totalData, ref int totalRows);
    }
}