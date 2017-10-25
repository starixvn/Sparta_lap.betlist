namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Extension.Utility.Interfaces;
    using Fanex.Athena.Resources;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Service.Core.Utility;
    using Fanex.Athena.Service.Core.Utility.Interfaces;
    using Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Newtonsoft.Json.Linq;

    public class NotificationReportService : INotificationReportService
    {
        private const string CacheNotificationTypeKey = "NotificationReport_NotificationType";

        private readonly IMemoryCacheService _cache;

        private readonly IAppSettings _appSetting;

        private readonly IWebClientService _webClientService;

        public NotificationReportService()
        {
            _cache = new MemoryCacheService();
            _appSetting = new AppSettings();
            _webClientService = new WebClientService();
        }

        public IDictionary<string, string> GetNotificationTypes()
        {
            var data = _cache.Get<IDictionary<string, string>>(CacheNotificationTypeKey);
            if (data == null)
            {
                data = GetNotificationTypeFromApi();
                _cache.Set(CacheNotificationTypeKey, data);
            }

            return data;
        }

        private IDictionary<string, string> GetNotificationTypeFromApi()
        {
            var notificationTypeUrl = _appSetting.NotificationTypeApiUrl;

            var json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(notificationTypeUrl);
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception, "ApiUrl: " + notificationTypeUrl);
            }

            if (string.IsNullOrEmpty(json))
            {
                return new Dictionary<string, string>();
            }

            var jobject = JObject.Parse(json);
            if (jobject.HasValues)
            {
                jobject.AddFirst(new JProperty("0", ViewResources.All));
            }

            return jobject.ToObject<Dictionary<string, string>>();
        }

        public ReportResultModel<Notification> GetNotifications(NotificationMetaModel metaModel)
        {
            var notificationUrl = string.Format(
                _appSetting.NotificationApiUrl,
                metaModel.Type,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.UserId,
                metaModel.LicenseeName,
                metaModel.IsOnlyUnread,
                metaModel.PageSize,
                metaModel.PageIndex);

            var json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(notificationUrl);
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception, "ApiUrl: " + notificationUrl);
            }

            var dataList = new List<Notification>();
            var totalPage = 0;

            var jobject = JObject.Parse(json);
            if (jobject.HasValues)
            {
                var lang = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(metaModel.Language.ToLowerInvariant());
                var subjectKey = "Subject" + lang;
                var contentKey = "Content" + lang;

                dataList.AddRange(jobject["Data"].Children().Select(x => new Notification
                {
                    DateTime = x["Date"].Value<DateTime>(),
                    Type = x["Type"].Value<string>(),
                    Content = x[contentKey].Value<string>(),
                    Subject = x[subjectKey].Value<string>()
                }));

                totalPage = jobject["TotalPage"].Value<int>();
            }

            return new ReportResultModel<Notification>
            {
                DataList = dataList,
                TotalRows = totalPage
            };
        }

        public ReportResultModel<ResultAmendment> GetResultAmendment(NotificationMetaModel metaModel)
        {
            var resultAmendmentUrl = string.Format(
                _appSetting.ResultAmendmentApiUrl,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.UserId,
                metaModel.LicenseeName,
                metaModel.IsOnlyUnread,
                metaModel.PageSize,
                metaModel.PageIndex);

            var json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(resultAmendmentUrl);
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception, "ApiUrl: " + resultAmendmentUrl);
            }

            var dataList = new List<ResultAmendment>();
            var totalPage = 0;

            var jobject = JObject.Parse(json);
            if (jobject.HasValues)
            {
                var lang = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(metaModel.Language.ToLowerInvariant());
                var subjectKey = "Subject" + lang;
                var contentKey = "Content" + lang;

                dataList.AddRange(jobject["Data"].Children().Select(x => new ResultAmendment
                {
                    DateTime = x["Date"].Value<DateTime>(),
                    Content = x[contentKey].Value<string>(),
                    Subject = x[subjectKey].Value<string>(),
                    DataFilePath = x["DataFilePath"].Value<string>()
                }));

                totalPage = jobject["TotalPage"].Value<int>();
            }

            return new ReportResultModel<ResultAmendment>
            {
                DataList = dataList,
                TotalRows = totalPage
            };
        }
    }
}