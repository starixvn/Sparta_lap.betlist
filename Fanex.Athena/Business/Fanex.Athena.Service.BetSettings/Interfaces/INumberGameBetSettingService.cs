// -----------------------------------------------------------------------
// <copyright file="INumberGameBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface INumberGameBetSettingService
    {
        BetSettingViewModel GetViewModel(int custId);

        IResultInfo UpdateBetSetting(CommonBetSetting setting, Hashtable data);
    }
}