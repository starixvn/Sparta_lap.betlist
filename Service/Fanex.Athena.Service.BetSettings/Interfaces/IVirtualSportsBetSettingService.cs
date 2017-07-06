// -----------------------------------------------------------------------
// <copyright file="IVirtualSportsBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IVirtualSportsBetSettingService
    {
        BetSettingViewModel GetViewModel(int licenseeId, int custId, int productId);

        IResultInfo UpdateBetSetting(int licenseeId, int custId, int userId, string ip, IEnumerable<VSBetSetting> betSettings);
    }
}