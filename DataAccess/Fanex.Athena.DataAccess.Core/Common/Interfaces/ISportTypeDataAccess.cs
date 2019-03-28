// -----------------------------------------------------------------------
// <copyright file="ISportTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ISportTypeDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        Task<IDictionary<int, string>> GetAllSports();

        Task<IEnumerable<SportProductItem>> GetSportsAndProducts(int licenseeId, string languageCode, string productCodes = "");
    }
}