// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Session.Interfaces;

    public class CoreBetSettingDataAccess : BaseDataAccess, ICoreBetSettingDataAccess
    {
        public CoreBetSettingDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId)
        {
            var uspName = "Lrf_LicenseeProductSel";
            using (var db = this.GetObjectDb(uspName))
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