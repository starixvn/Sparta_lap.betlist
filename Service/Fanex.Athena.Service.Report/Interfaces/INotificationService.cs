namespace Fanex.Athena.Service.Report.Interfaces
{
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface INotificationService
    {
        ReportResultModel<NotificationMessage> GetNotificationMessages(NotificationMetaModel metaModel);

        ReportResultModel<ResultAmendment> GetResultAmendment(NotificationMetaModel metaModel);

        IResultInfo MarkNotificationAsRead(NotificationMarkAsReadMetaModel metaModel);

        IResultInfo GetFileAttachmentData(NotificationAttachmentMetaModel metaModel);
    }
}