namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Resources;
    using Fanex.Athena.Service.Core.Utility.Interfaces;
    using Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Newtonsoft.Json.Linq;

    public class NotificationService : INotificationService
    {
        private readonly IDictionary<string, string> _notificationType = new Dictionary<string, string>
        {
            { nameof(ResultAmendment), "Amendment" }
        };

        private readonly IDictionary<string, string> _supportedLanguage = new Dictionary<string, string>
        {
            { "en", "En" },
            { "ch", "Ch" },
            { "cs", "Cs" },
            { "zhcn", "Cs" }
        };

        private readonly ICoreLogger _logger;

        private readonly IAppSettings _appSetting;

        private readonly IWebClientService _webClientService;

        [CLSCompliant(false)]
        public NotificationService(
            ICoreLogger logger,
            IAppSettings appSetting,
            IWebClientService webClientService)
        {
            _logger = logger;
            _appSetting = appSetting;
            _webClientService = webClientService;
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

        public ReportResultModel<ResultAmendment> GetResultAmendment(NotificationMetaModel metaModel)
        {
            if (string.IsNullOrEmpty(_appSetting.ResultAmendmentApiUrl))
            {
                return new ReportResultModel<ResultAmendment> { DataList = new ResultAmendment[0] };
            }

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
                    return new ReportResultModel<ResultAmendment> { DataList = dataList };
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

        private void LogError(string api, Exception exception)
        {
            _logger.Error(exception, exception.Message + "\nApiUrl: " + api);
        }
    }
}