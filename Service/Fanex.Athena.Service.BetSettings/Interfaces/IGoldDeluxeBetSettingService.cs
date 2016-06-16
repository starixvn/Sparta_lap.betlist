namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IGoldDeluxeBetSettingService
    {
        GoldDeluxeBetSettingViewModel GetViewModel(int licenseeId, int custId, string siteName);

        IResultInfo UpdateBetSetting(int licenseeId, string siteName, int custId, string group);

        IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}