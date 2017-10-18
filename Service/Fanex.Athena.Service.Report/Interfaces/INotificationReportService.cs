namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;

    public interface INotificationReportService
    {
        IDictionary<string, string> GetNotificationTypes();
    }
}