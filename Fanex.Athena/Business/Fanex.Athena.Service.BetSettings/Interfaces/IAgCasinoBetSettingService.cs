// -----------------------------------------------------------------------
// <copyright file="IAgCasinoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.DTO;

    public interface IAgCasinoBetSettingService
    {
        AgCasinoBetSettingViewModel GetViewModel(int custId, string licenseeName);

        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group);

        IResultInfo UpdateStatus(int userId, int custId, string language, string ip, bool isEnabled);
    }
}