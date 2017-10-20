namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Report.DTO;

    public interface INotificationReportService
    {
        IDictionary<string, string> GetNotificationTypes();

        IList<Notification> GetNotifications();

        IList<ResultAmendment> GetResultAmendment();
    }
}