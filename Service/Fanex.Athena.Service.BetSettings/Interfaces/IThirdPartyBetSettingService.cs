namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IThirdPartyBetSettingService
    {
        ThirdPartyBetSettingViewModel GetViewModel(int licenseeId, int custId, int productid);

        IResultInfo UpdateStatus(int licenseeId, int productId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}