namespace Fanex.Athena.Service.Interface.BetSettings
{
    using System.Collections;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface INumberGameBetSettingService
    {
        BetSettingViewModel GetViewModel(int custId);

        IResultInfo UpdateBetSetting(CommonBetSetting setting, Hashtable data);
    }
}