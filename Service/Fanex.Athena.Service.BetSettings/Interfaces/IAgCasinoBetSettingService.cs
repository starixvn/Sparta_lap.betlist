// -----------------------------------------------------------------------
// <copyright file="IAgCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.DTO;

    public interface IAgCasinoBetSettingService
    {
        Task<AgCasinoBetSettingViewModel> GetViewModel(int licenseeId, int custId, string licenseeName);

        Task<IResultInfo> UpdateAgCasinoBetSetting(CommonBetSetting setting, string group);

        Task<IResultInfo> UpdateStatus(int licenseeId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}