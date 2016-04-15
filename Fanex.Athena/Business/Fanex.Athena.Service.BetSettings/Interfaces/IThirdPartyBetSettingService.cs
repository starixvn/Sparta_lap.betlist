namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface IThirdPartyBetSettingService
    {
        ThirdPartyBetSettingViewModel GetViewModel(int custId, int productid);

        IResultInfo UpdateStatus(int productId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}