namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface INotificationReportService
    {
        IDictionary<string, string> GetNotificationTypes();

        ReportResultModel<NotificationMessage> GetNotificationMessages(NotificationMetaModel metaModel);

        ReportResultModel<Notification> GetGeneral(NotificationMetaModel metaModel);

        ReportResultModel<ResultAmendment> GetResultAmendment(NotificationMetaModel metaModel);

        IResultInfo MarkNotificationAsRead(int userId, string licenseeName, NotificationMessage metaModel);
    }
}