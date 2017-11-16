namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface INotificationService
    {
        IDictionary<string, string> GetNotificationTypes();

        ReportResultModel<NotificationMessage> GetNotificationMessages(NotificationMetaModel metaModel);

        ReportResultModel<GeneralNotification> GetGeneral(NotificationMetaModel metaModel);

        ReportResultModel<ResultAmendment> GetResultAmendment(NotificationMetaModel metaModel);

        IResultInfo MarkNotificationAsRead(NotificationMarkAsReadMetaModel metaModel);
    }
}