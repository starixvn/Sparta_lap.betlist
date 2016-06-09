// -----------------------------------------------------------------------
// <copyright file="IVirtualSportsBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IVirtualSportsBetSettingService
    {
        BetSettingViewModel GetViewModel(int custId);

        IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, IEnumerable<VSBetSetting> betSettings);
    }
}