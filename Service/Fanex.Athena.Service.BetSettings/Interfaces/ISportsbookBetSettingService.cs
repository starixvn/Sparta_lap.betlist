// -----------------------------------------------------------------------
// <copyright file="ISportsbookBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ISportsbookBetSettingService
    {
        Task<BetSettingViewModel> GetViewModelAsync(int licenseeId, int custId);

        Task<IResultInfo> UpdateBetSettingAsync(CommonBetSetting setting, IDictionary<string, object> hashParam, DcsBetSetting esportSetting);
    }
}