using Fanex.Athena.Session.Model;

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    public interface ILicenseeSettingService
    {
        bool IsFakeLicensee();

        bool IsAuthorizedOnBetList();

        FakeLicenseeSetting GetFakeLicenseeSetting();
    }
}