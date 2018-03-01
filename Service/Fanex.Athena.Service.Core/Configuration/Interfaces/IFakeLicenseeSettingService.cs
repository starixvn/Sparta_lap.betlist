using Fanex.Athena.Model.Core.DTO.Configuration;

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    public interface IFakeLicenseeSettingService
    {
        FakeLicenseeSetting GetSetting(string licenseeName);
    }
}