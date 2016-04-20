// -----------------------------------------------------------------------
// <copyright file="BetTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetTypeDataAccess : BaseDataAccess, IBetTypeDataAccess
    {
        /// <summary>
        /// Get bet type.
        /// </summary>
        /// <param name="licenseeId">The licensee Id.</param>
        /// <param name="sports">The sports (xml string parameter).</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The data table.</returns>
        public IList<CheckedItem> GetBettypes(int licenseeId, string sports, string languageCode)
        {
            const string UspName = "Lrf_SelBetTypeBySport";
            IList<CheckedItem> results = new List<CheckedItem>();
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                    db.AddInParameter("sportlist", DbType.String, sports);
                    db.AddInParameter("langkey", DbType.String, languageCode);
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new CheckedItem
                            {
                                Checked = true,
                                ID = int.Parse(reader["BettypeID"].ToString()),
                                Name = reader["BettypeName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }

            return results;
        }

        /// <summary>
        /// Get all bet type.
        /// </summary>
        /// <param name="productOptions">Product option value.</param>
        /// <returns>The result value.</returns>
        public IDictionary<string, int> GetAllBettypes(IList<ProductOption> productOptions)
        {
            const string UspName = "Lrf_BetTypesGetAll";
            var result = new Dictionary<string, int>();

            using (var db = new ObjectDb(UspName))
            {
                foreach (var item in productOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bettypeId = reader.ToInt("Bettype");
                        var bettypeName = CacheRef.GetBettype(bettypeId);

                        if (!result.ContainsKey(reader.ToString("BettypeName")))
                        {
                            result.Add(bettypeName, bettypeId);
                        }
                    }
                }
            }

            return result;
        }
    }
}