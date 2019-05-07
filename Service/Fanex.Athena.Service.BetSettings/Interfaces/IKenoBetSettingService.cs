// -----------------------------------------------------------------------
// <copyright file="IKenoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IKenoBetSettingService
    {
        Task<BetSettingViewModel> GetViewModelAsync(int licenseeId, int custId);

        Task<IResultInfo> UpdateBetSettingAsync(int licenseeId, int custId, int userId, string language, string ip, KenoBetSetting setting);
    }
}