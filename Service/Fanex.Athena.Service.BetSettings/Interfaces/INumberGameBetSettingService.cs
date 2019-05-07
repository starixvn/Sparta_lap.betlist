// -----------------------------------------------------------------------
// <copyright file="INumberGameBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface INumberGameBetSettingService
    {
        Task<BetSettingViewModel> GetViewModelAsync(int licenseeId, int custId);

        Task<IResultInfo> UpdateBetSettingAsync(CommonBetSetting setting, Hashtable data);
    }
}