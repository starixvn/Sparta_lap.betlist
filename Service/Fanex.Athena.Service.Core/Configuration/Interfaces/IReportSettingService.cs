using Fanex.Athena.Model.Core.DTO.Configuration;

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    public interface IReportSettingService
    {
        ReportSetting GetSetting(string reportKey);
    }
}