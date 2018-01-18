namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
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

        private readonly IDictionary<string, string> _supportedLanguage = new Dictionary<string, string>
        {
            { "en", "En" },
            { "ch", "Ch" },
            { "cs", "Cs" },
            { "zhcn", "Cs" }
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
                    var lang = GetSupportedLanguage(metaModel.Language);
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
                    var lang = GetSupportedLanguage(metaModel.Language);
                    var subjectKey = "Subject" + lang;
                    var contentKey = "Content" + lang;

                    totalRecords = jobject["TotalItems"].Value<int>();

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
                TotalRows = totalRecords
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

        public IResultInfo GetFileAttachmentData(NotificationAttachmentMetaModel metaModel)
        {
            var url = string.Format(_appSetting.ResultAmendmentAttachmentApiUrl, metaModel.FileName);
            var result = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.ErrorProcessingMsg
            };
            try
            {
                var json = _webClientService.DownloadString(url);
                if (string.IsNullOrEmpty(json))
                {
                    return result;
                }

                var jobject = JObject.Parse(json);

                if (jobject.HasValues)
                {
                    return new ResultInfo
                    {
                        ExtendedData = jobject["content"].Value<string>(),
                        Code = jobject["result"]["errorCode"].Value<int>(),
                        Message = jobject["result"]["message"].Value<string>()
                    };
                }
            }
            catch (Exception e)
            {
                LogError(url, e);
            }

            return result;
        }

        private string GetSupportedLanguage(string lang)
        {
            var key = lang.ToLowerInvariant();
            if (_supportedLanguage.ContainsKey(key))
            {
                return _supportedLanguage[key];
            }

            return _supportedLanguage.First().Value;
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
                DateTime = token["EventDate"].Value<DateTime>(),
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