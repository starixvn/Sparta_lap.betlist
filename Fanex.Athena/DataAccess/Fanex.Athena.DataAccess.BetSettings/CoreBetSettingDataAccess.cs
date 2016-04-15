// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core;
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