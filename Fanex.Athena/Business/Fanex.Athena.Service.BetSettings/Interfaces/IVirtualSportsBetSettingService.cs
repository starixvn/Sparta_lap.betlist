// -----------------------------------------------------------------------
// <copyright file="IVirtualSportsBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface IVirtualSportsBetSettingService
    {
        BetSettingViewModel GetViewModel(int custId);

        IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, IEnumerable<VSBetSetting> betSettings);
    }
}