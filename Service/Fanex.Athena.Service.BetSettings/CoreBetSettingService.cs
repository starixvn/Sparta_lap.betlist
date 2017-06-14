// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.BetSettings;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Service.BetSettings.Interfaces;

    public class CoreBetSettingService : ICoreBetSettingService
    {
        private readonly ICoreBetSettingDataAccess _coreBetSettingDataAccess;

        public CoreBetSettingService()
        {
            _coreBetSettingDataAccess = new CoreBetSettingDataAccess();
        }

        public CoreBetSettingService(ICoreBetSettingDataAccess coreBetSettingDataAccess)
        {
            _coreBetSettingDataAccess = coreBetSettingDataAccess;
        }

        public IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId)
        {
            return _coreBetSettingDataAccess.GetBetSettingProducts(licenseeId);
        }
    }
}