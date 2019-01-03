// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Setting class.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        internal const string HttpSlashSlash = "http://";
        internal const char UrlSlash = '/';

        #region Configurations

        public bool IsDisableExportLargeBetList
        {
            get
            {
                return GetValue("IsDisableExportLargeBetList") == "1";
            }
        }

        public IEnumerable<int> LicenseesWithLargeData
        {
            get
            {
                return GetIntegers("LicenseesWithLargeData");
            }
        }

        public IEnumerable<int> LicenseeOfferNewForecast
        {
            get
            {
                return GetIntegers("LicenseeOfferNewForecast");
            }
        }

        public IEnumerable<int> LicenseeAllowCasinoTransfer
        {
            get
            {
                return GetIntegers("LicenseeAllowCasinoTransfer");
            }
        }

        public IEnumerable<int> LicenseeOfferStatusFunction
        {
            get
            {
                return GetIntegers("LicenseeOfferStatusFunction");
            }
        }

        public string ExchangeProductApiHost
        {
            get { return GetValue("ExchangeProductApiHost"); }
        }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string ResultAmendmentApiUrl
        {
            get { return GetValue(nameof(ResultAmendmentApiUrl)); }
        }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string MarkNotificationAsReadApiUrl
        {
            get { return GetValue(nameof(MarkNotificationAsReadApiUrl)); }
        }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string ResultAmendmentAttachmentApiUrl
        {
            get { return GetValue(nameof(ResultAmendmentAttachmentApiUrl)); }
        }

        public IEnumerable<int> FakeLicenseeList
        {
            get { return GetValue("FakeLicenseeList").Split(',').Select(n => Convert.ToInt32(n)); }
        }

        public bool IsEnabledMoveBetNotification
        {
            get { return int.Parse(GetValue("IsEnabledMoveBetNotification")) != 0; }
        }

        public string Environment
        {
            get { return GetValue("Environment"); }
        }

        public bool EnableSessionValidatingService
        {
            get { return int.Parse(GetValue("EnableSessionValidatingService")) != 0; }
        }

        [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string RemoteRefUrl
        {
            get { return GetValue("RemoteRefURL"); }
        }

        public bool LocalTest
        {
            get { return int.Parse(GetValue("LocalTest")) != 0; }
        }

        public bool IsShowAllDataForSettleMatchCountReport
        {
            get { return int.Parse(GetValue("IsShowAllDataForSettleMatchCountReport")) != 0; }
        }

        public bool IsShowMixedSport
        {
            get { return int.Parse(GetValue("IsShowMixedSport")) == 1; }
        }

        public DateTime HistoryDate
        {
            get { return DateTime.Parse(GetValue("HistoryDate")); }
        }

        public DateTime MaxDate
        {
            get
            {
                var dateString = GetValue("MaxDate");
                return string.IsNullOrWhiteSpace(dateString) ? DateTime.Now : DateTime.Parse(dateString);
            }
        }

        public string[] BasedCurrency
        {
            get { return GetValue("BasedCurrency").Split(new[] { ';' }); }
        }

        public IEnumerable<int> LiveReportNoDetailLinkSports
        {
            get { return GetValue("LiveReportNoDetailLinkSports").Split(',').Select(n => Convert.ToInt32(n)); }
        }

        #endregion Configurations

        #region Casino

        public bool IsDisableRngCasino
        {
            get { return bool.Parse(GetValue("IsDisableRngCasino")); }
        }

        #endregion Casino

        #region Assign Currency

        public bool EnableAssignCurrency
        {
            get { return bool.Parse(GetValue("EnableAssignCurrency")); }
        }

        public string TempFilePath => GetValue("TempFilePath");

        #endregion Assign Currency

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private IEnumerable<int> GetIntegers(string key)
        {
            var value = GetValue(key);
            return string.IsNullOrWhiteSpace(value) ? new int[] { } : value.Split(',').Select(n => Convert.ToInt32(n));
        }
    }
}