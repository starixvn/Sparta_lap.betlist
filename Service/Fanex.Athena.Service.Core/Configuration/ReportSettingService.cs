// -----------------------------------------------------------------------
// <copyright file="MoveBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Extension.Helper;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Interfaces;
    using Newtonsoft.Json;

    public class ReportSettingService : IReportSettingService
    {
        private const string CacheKey = "LRF_ReportSettings";

        private readonly IMemoryCacheService _cache;

        private readonly IAppSettings _appSettings;

        private readonly ILicenseeSettingService _licenseeSettingService;

        public ReportSettingService(IMemoryCacheService cache, IAppSettings appSettings, ILicenseeSettingService licenseeSettingService)
        {
            _cache = cache;
            _appSettings = appSettings;
            _licenseeSettingService = licenseeSettingService;
        }

        public ReportSetting GetSetting(string reportKey)
        {
            var settings = _cache.Get<IEnumerable<ReportSetting>>(CacheKey);
            if (settings == null)
            {
                settings = GetSettingList();
                _cache.Set(CacheKey, settings);
            }

            var reportSetting = settings.FirstOrDefault(x => x.ReportKey.Equals(reportKey, StringComparison.OrdinalIgnoreCase));
            if (reportSetting != null)
            {
                AssignFakeLicenseeSetting(reportSetting);
                AssignDateRange(reportSetting);
            }

            return reportSetting;
        }

        private static DateTime GetNowByTimeZone(int? timeZone)
        {
            return timeZone.HasValue ? DateTime.UtcNow.AddHours(timeZone.Value) : DateTime.Now;
        }

        private static DateTime ConvertToDate(string datetime, int? timeZone, DateTime? defaultDate)
        {
            if (string.IsNullOrEmpty(datetime))
            {
                return defaultDate.HasValue ? defaultDate.Value : GetNowByTimeZone(timeZone);
            }

            if (datetime.Equals("today", StringComparison.OrdinalIgnoreCase))
            {
                return GetNowByTimeZone(timeZone).Date;
            }

            if (datetime.Equals("yesterday", StringComparison.OrdinalIgnoreCase))
            {
                return GetNowByTimeZone(timeZone).Date.AddDays(-1);
            }

            return Convert.ToDateTime(datetime);
        }

        private void AssignDateRange(ReportSetting reportSetting)
        {
            reportSetting.MinDate = ConvertToDate(reportSetting.MinDateString, reportSetting.TimeZone, _appSettings.HistoryDate);
            reportSetting.MaxDate = ConvertToDate(reportSetting.MaxDateString, reportSetting.TimeZone, _appSettings.MaxDate);

            if (reportSetting.LimitRecentMonths > 0)
            {
                var limitedMinDate = GetNowByTimeZone(reportSetting.TimeZone).Date.AddMonths(-reportSetting.LimitRecentMonths + 1);
                limitedMinDate = new DateTime(limitedMinDate.Year, limitedMinDate.Month, 1);
                reportSetting.MinDate = DateTimeHelper.GetMaxDate(reportSetting.MinDate, limitedMinDate);
            }

            if (reportSetting.MaximumSupportedDays > 0)
            {
                var supportedMinDate = reportSetting.MaxDate.Date.AddDays(-reportSetting.MaximumSupportedDays);
                reportSetting.MinDate = DateTimeHelper.GetMaxDate(reportSetting.MinDate, supportedMinDate);
            }
        }

        private void AssignFakeLicenseeSetting(ReportSetting reportSetting)
        {
            var fakeLicenseeSetting = _licenseeSettingService.GetFakeLicenseeSetting();
            if (fakeLicenseeSetting != null)
            {
                reportSetting.MinDateString = fakeLicenseeSetting.MinDate;
                reportSetting.MaxDateString = fakeLicenseeSetting.MaxDate;
                reportSetting.IsDisableBetList = fakeLicenseeSetting.DisableBetList;
                reportSetting.BaseCurrencies = fakeLicenseeSetting.BaseCurrencies;
                reportSetting.LimitRecentMonths = fakeLicenseeSetting.LimitRecentMonths;
                reportSetting.TimeZone = fakeLicenseeSetting.TimeZone;
            }
        }

        private IEnumerable<ReportSetting> GetSettingList()
        {
            IEnumerable<ReportSetting> result = null;
            var jsonContent = GetSettingFileContent();

            if (!string.IsNullOrEmpty(jsonContent))
            {
                result = JsonConvert.DeserializeObject<IEnumerable<ReportSetting>>(jsonContent);
            }

            return result ?? new List<ReportSetting>();
        }

        protected internal virtual string GetSettingFileContent()
        {
            const string FilePath = "~/App_Data/ReportSetting.json";
            using (var reader = new StreamReader(HostingEnvironment.MapPath(FilePath)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}