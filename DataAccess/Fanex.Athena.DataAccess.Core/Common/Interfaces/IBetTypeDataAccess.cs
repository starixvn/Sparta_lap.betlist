// -----------------------------------------------------------------------
// <copyright file="IBetTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IBetTypeDataAccess
    {
        Task<IList<CheckedItem>> GetBettypes(int licenseeId, string xmlSportList, string languageCode);

        Task<IList<CheckedItem>> GetBettypesByProduct(int licenseeId, string productCode, string languageCode);
    }
}