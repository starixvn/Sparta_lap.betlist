// -----------------------------------------------------------------------
// <copyright file="IAgCasinoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.ViewModel.BetSettings;
    using Models.DTO.BetSettings;
    using Models.DTO.BetSettings.AgCasino;

    public interface IAgCasinoBetSettingService
    {
        AgCasinoBetSettingViewModel GetViewModel(int custId, string licenseeName);
        
        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group);

        IResultInfo UpdateStatus(int userId, int custId, string language, string ip, bool isEnabled);
    }
}