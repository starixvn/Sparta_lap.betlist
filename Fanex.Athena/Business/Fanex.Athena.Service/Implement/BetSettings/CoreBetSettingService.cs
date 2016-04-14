// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.BetSettings;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Service.Interface.BetSettings;

    public class CoreBetSettingService : ICoreBetSettingService
    {
        private readonly CoreBetSettingDataAccess _coreBetSettingDataAccess;

        public CoreBetSettingService()
        {
            _coreBetSettingDataAccess = new CoreBetSettingDataAccess();
        }

        public IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId)
        {
            return _coreBetSettingDataAccess.GetBetSettingProducts(licenseeId);
        }
    }
}