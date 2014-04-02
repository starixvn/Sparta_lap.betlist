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

        public SportTypeService(BalanceUp balanceUp)
        {
            _sportTypeDataAcess = new SportTypeDataAccess(balanceUp);
        }

        public IDictionary<string, int> GetAllSports()
        {
            return this._sportTypeDataAcess.GetAllSports();
        }

        /// <summary>
        ///     The get sport by date.
        /// </summary>
        /// <param name="fromdate">
        ///     The from date.
        /// </param>
        /// <param name="todate">
        ///     The to date.
        /// </param>
        /// <returns>
        /// The sport list.
        /// </returns>
        public IList<Sport> GetAllWHSports()
        {
            return this._sportTypeDataAcess.GetAllWHSports();
        }

        /// <summary>
        /// Get products and sports.
        /// </summary>
        /// <param name="licenseeid">The licensee id.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="productId">Product ID.</param>
        /// <returns>The products and sports.</returns>
        public IList<SportProductItem> GetSportsAndProducts(int licenseeid, string languageCode, int? productId = null)
        {
            var result = new List<SportProductItem>();
            var tableData = _sportTypeDataAcess.GetSportsAndProducts(licenseeid, languageCode);
            if (tableData != null && tableData.Rows.Count > 0)
            {
                if (productId.HasValue)
                {
                    tableData = tableData.Select("ParentID = " + productId).CopyToDataTable();
                }
                
                foreach (DataRow row in tableData.Rows)
                {
                    result.Add(new SportProductItem
                    {
                        Checked = true,
                        ID = int.Parse(row["ID"].ToString()),
                        Name = row["Name"].ToString(),
                        ParentId = row["ParentId"] != DBNull.Value ? int.Parse(row["ParentId"].ToString()) : -1
                    });
                }
            }

            return result;
        }
    }
}