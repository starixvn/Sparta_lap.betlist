// -----------------------------------------------------------------------
// <copyright file="IBetTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IBetTypeDataAccess
    {
        IList<CheckedItem> GetBettypes(int licenseeId, string xmlSportList, string languageCode);

        IList<CheckedItem> GetBettypesByProduct(int licenseeId, string productCode, string languageCode);
    }
}