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
    using System.Linq;

    /// <summary>
    /// Setting class.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        internal const string HttpSlashSlash = "http://";
        internal const char UrlSlash = '/';

        #region Configurations

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1056:UriPropertiesShouldNotBeStrings",
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

        public string ClientViewType
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientViewType"];
            }
        }

        #endregion Assign Currency

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}