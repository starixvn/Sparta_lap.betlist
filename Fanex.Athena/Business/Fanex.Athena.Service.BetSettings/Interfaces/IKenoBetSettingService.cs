// -----------------------------------------------------------------------
// <copyright file="IKenoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface IKenoBetSettingService
    {
        BetSettingViewModel GetViewModel(int custId);

        IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, KenoBetSetting setting);
    }
}