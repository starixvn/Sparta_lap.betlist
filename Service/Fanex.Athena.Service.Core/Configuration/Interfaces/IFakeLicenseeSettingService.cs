using Fanex.Athena.Session.Model;

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    public interface IFakeLicenseeSettingService
    {
        bool IsFakeLicensee();

        FakeLicenseeSetting GetSetting();
    }
}