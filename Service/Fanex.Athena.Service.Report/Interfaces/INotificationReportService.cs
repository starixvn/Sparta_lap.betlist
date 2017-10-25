namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface INotificationReportService
    {
        IDictionary<string, string> GetNotificationTypes();

        ReportResultModel<Notification> GetNotifications(NotificationMetaModel metaModel);

        ReportResultModel<ResultAmendment> GetResultAmendment(NotificationMetaModel metaModel);
    }
}