// -----------------------------------------------------------------------
// <copyright file="SportTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SportTypeService : ISportTypeService
    {
        private readonly ISportTypeDataAccess _sportTypeDataAcess;

        public SportTypeService()
        {
            _sportTypeDataAcess = new SportTypeDataAccess();
        }

        public IDictionary<int, string> GetAllSports()
        {
            return this._sportTypeDataAcess.GetAllSports();
        }

        public IList<Sport> GetAllWHSports()
        {
            return this._sportTypeDataAcess.GetAllWHSports();
        }

        public IEnumerable<SportProductItem> GetSportsAndProducts(int licenseeId, string languageCode, int? productId = null)
        {
            return _sportTypeDataAcess.GetSportsAndProducts(licenseeId, languageCode, productId);
        }
    }
}