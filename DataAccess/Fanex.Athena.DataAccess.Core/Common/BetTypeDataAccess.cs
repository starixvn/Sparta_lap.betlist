// -----------------------------------------------------------------------
// <copyright file="BetTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
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
                using (var db = this.GetObjectDb(UspName))
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
    }
}