// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;

    public class CoreBetSettingDataAccess : BaseDataAccess, ICoreBetSettingDataAccess
    {
        public async Task<IEnumerable<BetSettingProduct>> GetBetSettingProductsAsync(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeProductSel";
            using (var db = this.GetObjectDb(UspName))
            {
                var param = new Hashtable
                {
                    { "licenseeid", licenseeId }
                };
                return await db.QueryAsync<BetSettingProduct>(param);
            }
        }
    }
}