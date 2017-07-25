// -----------------------------------------------------------------------
// <copyright file="IRacingBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;

    public interface IRacingBetSettingService
    {
        BetSettingViewModel GetViewModel(int licenseeId, int custId);

        IDictionary<string, string> UpdateBetSetting(int licenseeId, IDictionary<string, RacingBetSetting> racingParam);
    }
}