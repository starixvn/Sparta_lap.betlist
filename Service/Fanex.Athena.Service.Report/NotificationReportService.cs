namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Extension.Utility.Interfaces;
    using Fanex.Athena.Resources;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Service.Core.Utility;
    using Fanex.Athena.Service.Core.Utility.Interfaces;
    using Interfaces;
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
    }
}