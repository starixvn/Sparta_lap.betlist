namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IGoldDeluxeBetSettingService
    {
        GoldDeluxeBetSettingViewModel GetViewModel(int custId, string siteName);

        IResultInfo UpdateBetSetting(string siteName, int custId, string group);

        IResultInfo UpdateStatus(int userId, int custId, string language, string ip, bool isEnabled);
    }
}