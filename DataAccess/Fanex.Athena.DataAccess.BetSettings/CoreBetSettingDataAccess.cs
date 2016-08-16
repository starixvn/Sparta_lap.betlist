// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Data;

    public class CoreBetSettingDataAccess : BaseDataAccess, ICoreBetSettingDataAccess
    {
        public IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId)
        {
            var uspName = "Lrf_LicenseeProductSel";
            using (var db = new ObjectDb(uspName))
            {
                var param = new Hashtable
                {
                    { "licenseeid", licenseeId }
                };
                return db.Query<BetSettingProduct>(param);
            }
        }

        public bool IsBetSettingEditable(int licenseeId, int userId)
        {
            var uspName = "Lrf_CheckEditBetSetting";
            using (var db = new ObjectDb(uspName))
            {
                var param = new Hashtable
                {
                    { "licenseeid", licenseeId },
                    { "userid", userId }
                };
                using (var reader = db.ExecuteReader(param))
                {
                    if (reader.Read())
                    {
                        return (bool)reader["IsGrant"];
                    }
                }
            }

            return false;
        }
    }
}