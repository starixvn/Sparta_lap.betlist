namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Extension.Utility.Interfaces;
    using Fanex.Athena.Resources;
    using Fanex.Athena.Service.Core.Utility;
    using Fanex.Athena.Service.Core.Utility.Interfaces;
    using Interfaces;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Newtonsoft.Json.Linq;

    public class NotificationReportService : INotificationReportService
    {
        private const string CacheNotificationTypeKey = "NotificationReport_NotificationType";

        private readonly ILogger _logger;

        private readonly IMemoryCacheService _cache;

        private readonly IAppSettings _appSetting;

        private readonly IWebClientService _webClientService;

        public NotificationReportService() : this(null, null, null, null)
        {
        }

        [CLSCompliant(false)]
        public NotificationReportService(
            ILogger logger,
            IMemoryCacheService cache,
            IAppSettings appSetting,
            IWebClientService webClientService)
        {
            _logger = logger ?? Logger.Log;
            _cache = cache ?? new MemoryCacheService();
            _appSetting = appSetting ?? new AppSettings();
            _webClientService = webClientService ?? new WebClientService();
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
                _logger.Error(exception.Message + "\nApiUrl: " + notificationTypeUrl, exception);
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

        public ReportResultModel<NotificationMessage> GetNotificationMessages(NotificationMetaModel metaModel)
        {
            var result = GetResultAmendment(metaModel);
            var dataList = result.DataList.Select(x => new NotificationMessage
            {
                DateTime = x.DateTime,
                Subject = x.Subject,
                Content = x.Content,
                Type = "Amendment"
            }).ToList();

            return new ReportResultModel<NotificationMessage>
            {
                DataList = dataList,
                TotalRows = result.TotalRows
            };
        }

        public ReportResultModel<Notification> GetGeneral(NotificationMetaModel metaModel)
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
                metaModel.PageIndex - 1);

            var json = GetApiResultString(notificationUrl);

            var dataList = new List<Notification>();
            var totalRecords = 0;

            if (string.IsNullOrEmpty(json))
            {
                return new ReportResultModel<Notification>
                {
                    DataList = dataList,
                    TotalRows = totalRecords
                };
            }

            var jobject = JObject.Parse(json);
            if (jobject.HasValues && jobject["Data"].Children().Any())
            {
                var lang = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(metaModel.Language.ToLowerInvariant());
                var subjectKey = "Subject" + lang;
                var contentKey = "Content" + lang;

                dataList.AddRange(jobject["Data"].Children().Select(x => new Notification
                {
                    DateTime = x["Date"].Value<DateTime>(),
                    MessageTypeId = x["MessageTypeId"].Value<int>(),
                    Content = x[contentKey].Value<string>(),
                    Subject = x[subjectKey].Value<string>()
                }));

                totalRecords = jobject["TotalItems"].Value<int>();
            }

            return new ReportResultModel<Notification>
            {
                DataList = dataList,
                TotalRows = totalRecords
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
                metaModel.PageIndex - 1);

            var json = GetApiResultString(resultAmendmentUrl);

            var dataList = new List<ResultAmendment>();
            var totalRecords = 0;

            if (string.IsNullOrEmpty(json))
            {
                return new ReportResultModel<ResultAmendment>
                {
                    DataList = dataList,
                    TotalRows = totalRecords
                };
            }

            var jobject = JObject.Parse(json);
            if (jobject.HasValues && jobject["Data"].Children().Any())
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

                totalRecords = jobject["TotalItems"].Value<int>();
            }

            return new ReportResultModel<ResultAmendment>
            {
                DataList = dataList,
                TotalRows = totalRecords
            };
        }

        public IResultInfo MarkNotificationAsRead(int userId, string licenseeName, NotificationMessage metaModel)
        {
            var api = new Uri(_appSetting.MarkNotificationAsReadApiUrl);
            var param = $"MessageType={metaModel.Type}&MessageId={metaModel.Id}&UserId={userId}&LicenseeName={licenseeName}";

            var json = string.Empty;
            try
            {
                json = _webClientService.UploadString(api, param);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message + "\nApiUrl: " + api, exception);
            }

            var jobject = JObject.Parse(json);

            return new ResultInfo
            {
                Code = jobject["Code"].Value<int>(),
                Message = jobject["Message"].Value<string>()
            };
        }

        private string GetApiResultString(string api)
        {
            var json = string.Empty;

            try
            {
                json = _webClientService.DownloadString(api);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message + "\nApiUrl: " + api, exception);
            }

            return json;
        }
    }
}