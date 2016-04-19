// -----------------------------------------------------------------------
// <copyright file="IRacingBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;

    public interface IRacingBetSettingDataAccess
    {
        IList<BetSetting> GetBetSetting(int custId, bool isAllowSaToreCurrency);

        IDictionary<string, string> UpdateBetSetting(Dictionary<string, RacingBetSetting> racingBetSettingList);
    }
}