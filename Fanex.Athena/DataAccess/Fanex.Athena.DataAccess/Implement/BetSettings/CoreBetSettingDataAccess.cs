// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings;
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
    }
}