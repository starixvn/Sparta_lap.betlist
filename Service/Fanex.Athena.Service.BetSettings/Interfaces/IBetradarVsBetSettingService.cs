// -----------------------------------------------------------------------
namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.BetSettings.ViewModel;

    public interface IBetradarVsBetSettingService
    {
        BetSettingViewModel GetViewModel(int licenseeId, int custId);
    }
}