// -----------------------------------------------------------------------
// <copyright file="SportTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Common;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Service.Interface.Common;

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

        public IDictionary<string, int> GetAllSports()
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