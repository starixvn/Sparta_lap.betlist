// -----------------------------------------------------------------------
// <copyright file="IRacingBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;

    public interface IRacingBetSettingDataAccess
    {
        IList<BetSetting> GetBetSetting(int custId);

        IDictionary<string, string> UpdateBetSetting(IDictionary<string, RacingBetSetting> racingBetSettingList);
    }
}