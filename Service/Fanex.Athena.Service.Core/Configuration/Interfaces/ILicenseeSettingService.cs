using Fanex.Athena.Model.Core.DTO.Configuration;

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    public interface ILicenseeSettingService
    {
        bool IsFakeLicensee(string licenseeName);

        bool IsAuthorizedOnBetList(string licenseeName);

        FakeLicenseeSetting GetFakeLicenseeSetting(string licenseeName);

    }
}