// -----------------------------------------------------------------------
// <copyright file="CoreBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Service.BetSettings.Interfaces;

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