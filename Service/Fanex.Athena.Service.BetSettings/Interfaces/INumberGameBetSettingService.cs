// -----------------------------------------------------------------------
// <copyright file="INumberGameBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface INumberGameBetSettingService
    {
        BetSettingViewModel GetViewModel(int custId);

        IResultInfo UpdateBetSetting(CommonBetSetting setting, Hashtable data);
    }
}