namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
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

    public class NotificationService : INotificationService
    {
        private const string CacheNotificationTypeKey = "NotificationReport_NotificationType";

        private readonly IDictionary<string, string> _notificationType = new Dictionary<string, string>
        {
            { nameof(GeneralNotification), "General" },
            { nameof(ResultAmendment), "Amendment" }
        };

        private readonly ILogger _logger;

        private readonly IMemoryCacheService _cache;

        private readonly IAppSettings _appSetting;

        private readonly IWebClientService _webClientService;

        public NotificationService() : this(null, null, null, null)
        {
        }

        [CLSCompliant(false)]
        public NotificationService(
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

        public ReportResultModel<NotificationMessage> GetNotificationMessages(NotificationMetaModel metaModel)
        {
            var result = GetResultAmendment(metaModel);
            var dataList = result.DataList.Select(x => new NotificationMessage
            {
                Id = x.Id,
                DateTime = x.DateTime,
                Subject = x.Subject,
                Content = x.Content,
                Type = _notificationType[nameof(ResultAmendment)]
            }).ToList();

            return new ReportResultModel<NotificationMessage>
            {
                DataList = dataList,
                TotalRows = result.TotalRows
            };
        }

        public ReportResultModel<GeneralNotification> GetGeneral(NotificationMetaModel metaModel)
        {
            var notificationUrl = string.Format(
                _appSetting.NotificationApiUrl,
                metaModel.Type,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.UserName,
                metaModel.LicenseeName,
                metaModel.IsOnlyUnread,
                metaModel.PageSize,
                metaModel.PageIndex - 1);
            var dataList = new List<GeneralNotification>();
            var totalRecords = 0;
            try
            {
                var json = _webClientService.DownloadString(notificationUrl);
                if (string.IsNullOrEmpty(json))
                {
                    return new ReportResultModel<GeneralNotification>
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

                    dataList.AddRange(jobject["Data"].Children().Select(x => MapToGeneralNotification(x, subjectKey, contentKey)));

                    totalRecords = jobject["TotalItems"].Value<int>();
                }
            }
            catch (Exception e)
            {
                LogError(notificationUrl, e);
            }

            return new ReportResultModel<GeneralNotification>
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
                metaModel.UserName,
                metaModel.LicenseeName,
                metaModel.IsOnlyUnread,
                metaModel.PageSize,
                metaModel.PageIndex - 1);
            var dataList = new List<ResultAmendment>();
            var totalRecords = 0;
            var downloadFileApi = string.Empty;

            try
            {
                var json = _webClientService.DownloadString(resultAmendmentUrl);
                if (string.IsNullOrEmpty(json))
                {
                    return new ReportResultModel<ResultAmendment>
                    {
                        DataList = dataList
                    };
                }

                var jobject = JObject.Parse(json);
                if (jobject.HasValues && jobject["Data"].Children().Any())
                {
                    var lang = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(metaModel.Language.ToLowerInvariant());
                    var subjectKey = "Subject" + lang;
                    var contentKey = "Content" + lang;

                    totalRecords = jobject["TotalItems"].Value<int>();
                    downloadFileApi = jobject["AttachmentFileApi"].Value<string>();

                    dataList.AddRange(jobject["Data"].Children().Select(x => MapToResultAmendment(x, subjectKey, contentKey)));                   
                }
            }
            catch (Exception e)
            {
                LogError(resultAmendmentUrl, e);
            }

            return new ReportResultModel<ResultAmendment>
            {
                DataList = dataList,
                TotalRows = totalRecords,
                AdditionalInfo = downloadFileApi
            };
        }

        public IResultInfo MarkNotificationAsRead(NotificationMarkAsReadMetaModel metaModel)
        {
            var api = new Uri(_appSetting.MarkNotificationAsReadApiUrl);
            var param = $"MessageType={metaModel.MessageType}&MessageId={metaModel.MessageId}&UserName={metaModel.UserName}&LicenseeName={metaModel.LicenseeName}";

            var result = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.ErrorProcessingMsg
            };
            try
            {
                var json = _webClientService.UploadString(api, param);
                if (string.IsNullOrEmpty(json))
                {
                    return result;
                }

                var jobject = JObject.Parse(json);

                if (jobject.HasValues)
                {
                    return new ResultInfo
                    {
                        Code = jobject["Code"].Value<int>(),
                        Message = jobject["Message"].Value<string>()
                    };
                }
            }
            catch (Exception e)
            {
                LogError(api + "\nParameter: " + param, e);
            }

            return result;
        }

        public byte[] GetFileAttachmentData(string api, string fileName)
        {
            try
            {
                var url = $"{api}?site=SpecialDelete&name={fileName}";
                return _webClientService.DownloadData(url);
            }
            catch (Exception e)
            {
                LogError(api, e);
            }

            return null;
        }

        private IDictionary<string, string> GetNotificationTypeFromApi()
        {
            var notificationTypeUrl = _appSetting.NotificationTypeApiUrl;

            try
            {
                var json = _webClientService.DownloadString(notificationTypeUrl);

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
            catch (Exception e)
            {
                LogError(notificationTypeUrl, e);
            }

            return new Dictionary<string, string>();
        }

        private ResultAmendment MapToResultAmendment(JToken token, string subjectKey, string contentKey)
        {           
            return new ResultAmendment
            {
                Id = token["MessageId"].Value<int>(),
                DateTime = token["Date"].Value<DateTime>(),
                Content = token[contentKey].Value<string>(),
                Subject = token[subjectKey].Value<string>(),
                DataFilePath = token["DataFilePath"].Value<string>(),
                IsRead = token["IsRead"].Value<bool>(),
                Type = _notificationType[nameof(ResultAmendment)]
            };
        }

        private GeneralNotification MapToGeneralNotification(JToken token, string subjectKey, string contentKey)
        {
            return new GeneralNotification
            {
                Id = token["MessageId"].Value<int>(),
                DateTime = token["Date"].Value<DateTime>(),
                MessageTypeId = token["MessageTypeId"].Value<int>(),
                Content = token[contentKey].Value<string>(),
                Subject = token[subjectKey].Value<string>(),
                IsRead = token["IsRead"].Value<bool>(),
                Type = _notificationType[nameof(GeneralNotification)]
            };
        }

        private void LogError(string api, Exception exception)
        {
            _logger.Error(exception.Message + "\nApiUrl: " + api, exception);
        }
    }
}